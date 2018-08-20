using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Artees.Diagnostics.BDD;
using Artees.Tools.DocCover;
using Xunit;

namespace DocCoverTest
{
    public class Tests
    {
        private static DocCoverReport GetReport()
        {
            using (var shouldListener = new NUnitShouldListener())
            {
                ShouldAssertions.Listeners.Add(shouldListener);
                var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "../../../../DocCoverTestAssembly/bin/Debug/netcoreapp2.1",
                    "DocCoverTestAssembly");
                var xmlPath = $"{projectPath}.xml";
                var dllPath = $"{projectPath}.dll";
                return DocCover.GetReport(xmlPath, dllPath);
            }
        }

        [Fact]
        public void TestMembers()
        {
            var members = GetReport().Members;
            members.Count.Should().BeEqual(24);
            members.Count(member => member.IsPublic).Should().BeEqual(6);
            members.Count(m => m.IsPublic && m.IsDocumented).Should().BeEqual(2);
            members.Where(m => m.IsPublic && m.IsDocumented)
                .All(m => m.Name.Contains("PublicСoveredClass")).Should().BeTrue();
            members.Count(m => m.IsPublic && !m.IsDocumented).Should().BeEqual(4);
            members.Count(m => m.IsPublic && !m.IsDocumented && m.Type != MemberTypes.Constructor)
                .Should().BeEqual(2);
        }

        [Fact]
        public void TestCoverage()
        {
            GetReport().GetCoverage().Aka("Coverage").Should().BeEqual(0.5);
        }

        [Fact]
        public void TestHtml()
        {
            var report = GetReport();
            var html = report.GetHtml();
            html.Should().Contains("<!DOCTYPE html>");
            var undocumentedPublicMember = report.Members.FirstOrDefault(m =>
                m.IsPublic && !m.IsDocumented && m.Type != MemberTypes.Constructor);
            if (undocumentedPublicMember == null) return;
            html.Should().Contains(undocumentedPublicMember.Name);
        }

        [Fact]
        public void TestBadge()
        {
            GetReport().GetBadge().Should().Contains("<svg ");
        }
    }
}