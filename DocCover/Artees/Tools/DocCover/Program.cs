using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Artees.Diagnostics.BDD;
using CommandLine;

namespace Artees.Tools.DocCover
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            using (var shouldListener = new WarningShouldListener())
            {
                ShouldAssertions.Listeners.Add(shouldListener);
                using (var traceListener = new ConsoleTraceListener())
                {
                    Trace.Listeners.Add(traceListener);
                    Execute(args);
                }
            }
        }

        private static void Execute(IEnumerable<string> args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Execute).WithNotParsed(Fail);
        }

        private static void Execute(Options options)
        {
            var xmlPath = options.Xml;
            xmlPath.Aka("XML").Should().Not().BeNull();
            if (xmlPath == null) return;
            var dllPath = Path.GetFullPath(options.Dll);
            var report = DocCover.GetReport(xmlPath, dllPath);
            var html = report.GetHtml();
            var outputPath = Path.GetFullPath(options.Output);
            if (Directory.Exists(outputPath)) Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);
            File.WriteAllText(Path.Combine(outputPath, "index.html"), html);
            var reportTemplatePath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "report.css");
            File.Copy(reportTemplatePath, Path.Combine(outputPath, "report.css"));
            var badge = report.GetBadge(BadgeStyle.All[options.Style]);
            File.WriteAllText(Path.Combine(outputPath, "badge.svg"), badge);
        }

        private static void Fail(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                if (error.Tag == ErrorType.HelpRequestedError ||
                    error.Tag == ErrorType.VersionRequestedError) continue;
                ShouldAssertions.Fail(error.ToString());
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Options
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local, MemberCanBePrivate.Local
            [Option('x', "xml", Hidden = true)] public string XmlOption { private get; set; }

            [Value(0, MetaName = "-x, --xml", HelpText = "The XML document to be analyzed.")]
            public string XmlValue { private get; set; }

            public string Xml => XmlOption ?? XmlValue ?? Dll.Remove(Dll.Length - 4) + ".xml";

            [Option('d', "dll", Hidden = true)] public string DllOption { private get; set; }

            [Value(1, MetaName = "-d, --dll",
                HelpText = "The assembly file to be analyzed. " +
                           "If not specified, the path of the XML document will be used.")]
            public string DllValue { private get; set; }

            public string Dll => DllOption ?? DllValue ?? Xml.Remove(Xml.Length - 4) + ".dll";

            [Option('o', "outputdir", Hidden = true)]
            public string OutputOption { private get; set; }

            [Value(2, MetaName = "-o, --outputdir",
                HelpText = "The directory where the generated report should be saved.")]
            public string OutputValue { private get; set; }

            public string Output => OutputOption ?? OutputValue ?? "doc_cover";

            [Option('s', "badgestyle", Hidden = true)]
            public string StyleOption { private get; set; }

            [Value(3, MetaName = "-s, --badgestyle",
                HelpText =
                    "The style of the generated badge. The following styles are available: " +
                    "plastic, flat, flat-square, for-the-badge, popout, popout-square, " +
                    "social.")]
            public string StyleValue { private get; set; }

            public string Style => StyleOption ?? StyleValue ?? "flat";
        }
    }
}