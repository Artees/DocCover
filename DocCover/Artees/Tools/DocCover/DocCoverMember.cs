using System;
using System.Reflection;
using Artees.Tools.XmlDocumentationNameGetter;

namespace Artees.Tools.DocCover
{
    public class DocCoverMember : IComparable<DocCoverMember>
    {
        private readonly MemberInfo _memberInfo;

        public readonly bool IsPublic,
            IsDocumented;

        public DocCoverMember(MemberInfo member, bool isPublic, bool isDocumented)
        {
            _memberInfo = member;
            IsPublic = isPublic;
            IsDocumented = isDocumented;
        }

        public string Name => _memberInfo.GetXmlDocsName();

        public MemberTypes Type => _memberInfo.MemberType;

        public int CompareTo(DocCoverMember other)
        {
            return string.Compare(Name, other.Name, StringComparison.InvariantCulture);
        }
    }
}