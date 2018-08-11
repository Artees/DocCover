using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Artees.Diagnostics.BDD;
using Artees.Tools.DocCover;
using NUnit.Framework;

namespace DocCoverTest
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            _listener = new NUnitShouldListener();
            ShouldAssertions.Listeners.Add(_listener);
            var projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "../../../DocCoverTestAssembly/bin/Debug/DocCoverTestAssembly");
            var xmlPath = $"{projectPath}.xml";
            var dllPath = $"{projectPath}.dll";
            _report = DocCover.GetReport(xmlPath, dllPath);
        }

        [TearDown]
        public void TearDown()
        {
            _listener.Dispose();
        }

        private DocCoverReport _report;
        private ShouldListener _listener;

        [Test]
        public void TestBadge()
        {
            _report.GetBadge().Should().Contains("<svg ");
        }

        [Test]
        public void TestCoverage()
        {
            _report.GetCoverage().Aka("Coverage").Should().BeEqual(0.5);
        }

        [Test]
        public void TestHtml()
        {
            var html = _report.GetHtml();
            html.Should().Contains("<!DOCTYPE html>");
            var undocumentedPublicMember = _report.Members.FirstOrDefault(m =>
                m.IsPublic && !m.IsDocumented && m.Type != MemberTypes.Constructor);
            if (undocumentedPublicMember == null) return;
            html.Should().Contains(undocumentedPublicMember.Name);
        }

        [Test]
        public void TestMembers()
        {
            var members = _report.Members;
            members.Count.Should().BeEqual(24);
            members.Count(member => member.IsPublic).Should().BeEqual(6);
            members.Count(m => m.IsPublic && m.IsDocumented).Should().BeEqual(2);
            members.Where(m => m.IsPublic && m.IsDocumented)
                .All(m => m.Name.Contains("PublicСoveredClass")).Should().BeTrue();
            members.Count(m => m.IsPublic && !m.IsDocumented).Should().BeEqual(4);
            members.Count(m => m.IsPublic && !m.IsDocumented && m.Type != MemberTypes.Constructor)
                .Should().BeEqual(2);
        }
    }
}