using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Artees.Diagnostics.BDD;
using Artees.Tools.XmlDocumentationNameGetter;

namespace Artees.Tools.DocCover
{
    public static class DocCover
    {
        public static DocCoverReport GetReport(string xmlPath, string dllPath)
        {
            xmlPath.Aka("XML").Should().Not().BeNull();
            var report = new DocCoverReport();
            if (xmlPath == null) return report;
            var xml = new XmlDocument();
            xml.Load(xmlPath);
            var dll = Assembly.LoadFile(dllPath);
            report.AssemblyName = dll.GetName();
            var xmlMembers = new List<string>();
            var xmlMembersnodeList = xml.GetElementsByTagName("member");
            for (var i = 0; i < xmlMembersnodeList.Count; i++)
            {
                var xmlMember = xmlMembersnodeList.Item(i);
                xmlMembers.Add(xmlMember?.Attributes?["name"].Value);
            }

            CheckIfTypesAreDocumented(dll.ExportedTypes, xmlMembers, true, report);
            CheckIfTypesAreDocumented(dll.DefinedTypes, xmlMembers, false, report);
            xmlMembers.Count.Aka("Final").Should().BeEqual(0);
            return report;
        }

        private static void CheckIfTypesAreDocumented(IEnumerable<Type> types,
            List<string> xmlMembers, bool isPublic, DocCoverReport report)
        {
            foreach (var type in types)
            {
                var typeName = type.GetXmlDocsName();
                var success = xmlMembers.RemoveAll(s => s == typeName);
                report.Add(new DocCoverMember(type, isPublic, success > 0));
                const BindingFlags pub = BindingFlags.DeclaredOnly | BindingFlags.Public |
                                         BindingFlags.Instance | BindingFlags.Static;
                var publicMembers = type.GetMembers(pub);
                CheckIfMembersAreDocumented(publicMembers, xmlMembers, isPublic, report);
                const BindingFlags nonPublic = BindingFlags.DeclaredOnly | BindingFlags.NonPublic |
                                               BindingFlags.Instance | BindingFlags.Static;
                var nonPublicMembers = type.GetMembers(nonPublic);
                CheckIfMembersAreDocumented(nonPublicMembers, xmlMembers, false, report);
            }
        }

        private static void CheckIfMembersAreDocumented(IEnumerable<MemberInfo> members,
            List<string> xmlMembers, bool isPublic, DocCoverReport report)
        {
            foreach (var member in members)
            {
                var mName = member.GetXmlDocsName();
                var success = xmlMembers.RemoveAll(s => s.StartsWith(mName));
                report.Add(new DocCoverMember(member, isPublic, success > 0));
            }
        }
    }
}