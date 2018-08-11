using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Artees.Tools.DocCover
{
    public class DocCoverReport
    {
        private readonly List<DocCoverMember> _members = new List<DocCoverMember>();

        public IReadOnlyList<DocCoverMember> Members => _members;

        internal AssemblyName AssemblyName { private get; set; } =
            Assembly.GetExecutingAssembly().GetName();

        internal void Add(DocCoverMember member)
        {
            _members.Add(member);
        }

        public double GetCoverage()
        {
            var documented = Members.Count(m => m.IsPublic && m.Type != MemberTypes.Constructor &&
                                                m.IsDocumented);
            var total = Members.Count(m => m.IsPublic && m.Type != MemberTypes.Constructor);
            return (double) documented / total;
        }

        public string GetHtml()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(baseDirectory, "ReportTemplate.html");
            var template = File.ReadAllText(path);
            var assemblyName = GetAssemblyNameAndVersion(AssemblyName);
            var documented = Members.Count(m => m.IsPublic && m.IsDocumented);
            var undocumented = Members.Where(m => m.IsPublic && !m.IsDocumented &&
                                                  m.Type != MemberTypes.Constructor).ToList();
            undocumented.Sort();
            var undocumentedCount = undocumented.Count;
            var coverage = (double) documented / (documented + undocumentedCount);
            var memberPath = Path.Combine(baseDirectory, "MemberTemplate.html");
            var memberTemplate = File.ReadAllText(memberPath);
            var membersList = undocumented.Count > 0
                ? undocumented.Select(m => string.Format(memberTemplate, m.Name))
                : new List<string> {"N/A"};
            var membersHtml = string.Join(string.Empty, membersList);
            var generatorAssemblyName = Assembly.GetExecutingAssembly().GetName();
            var generator = GetAssemblyNameAndVersion(generatorAssemblyName);
            var html = string.Format(template, assemblyName, DateTime.Now, documented,
                undocumentedCount, coverage, membersHtml, generator);
            return html;
        }

        private static string GetAssemblyNameAndVersion(AssemblyName assemblyName)
        {
            return $"{assemblyName.Name} {assemblyName.Version}";
        }

        public string GetBadge()
        {
            return GetBadge(BadgeStyle.Flat);
        }

        public string GetBadge(BadgeStyle style)
        {
            using (var client = new WebClient())
            {
                var coverage = Math.Floor(GetCoverage() * 100);
                var color = GetBadgeColor(coverage);
                var url = "https://img.shields.io/badge/" +
                          $"documented-{coverage}%-{color}.svg?style={style}";
                return client.DownloadString(url);
            }
        }

        private static string GetBadgeColor(double coverage)
        {
            var colorIndex = (int) Math.Floor(coverage / (100.0 / 6.0));
            switch (colorIndex)
            {
                case 0: return "red";
                case 1: return "orange";
                case 2: return "yellow";
                case 3: return "yellowgreen";
                case 4: return "green";
                case 5: return "brightgreen";
                case 6: return "brightgreen";
                default: return "lightgrey";
            }
        }
    }
}