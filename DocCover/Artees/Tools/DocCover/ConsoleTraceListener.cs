using System;
using System.Diagnostics;

namespace Artees.Tools.DocCover
{
    /// <inheritdoc />
    /// <summary>Directs tracing or debugging output to either the standard output or the standard
    /// error stream.</summary>
    internal class ConsoleTraceListener : TextWriterTraceListener
    {
        /// <inheritdoc />
        /// <summary>Initializes a new instance of the
        /// <see cref="T:System.Diagnostics.ConsoleTraceListener" /> class with trace output
        /// written to the standard output stream.</summary>
        public ConsoleTraceListener()
            : base(Console.Out)
        {
        }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the
        /// <see cref="T:System.Diagnostics.ConsoleTraceListener" /> class with an option to write
        /// trace output to the standard output stream or the standard error stream.</summary>
        /// <param name="useErrorStream">
        /// <see langword="true" /> to write tracing and debugging output to the standard error
        /// stream; <see langword="false" /> to write tracing and debugging output to the standard
        /// output stream.</param>
        // ReSharper disable once UnusedMember.Global
        public ConsoleTraceListener(bool useErrorStream)
            : base(useErrorStream ? Console.Error : Console.Out)
        {
        }

        /// <inheritdoc />
        /// <summary>Closes the output to the stream specified for this trace listener.</summary>
        public override void Close()
        {
        }
    }
}