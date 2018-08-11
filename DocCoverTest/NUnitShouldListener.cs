﻿using Artees.Diagnostics.BDD;
using NUnit.Framework;

namespace DocCoverTest
{
    internal class NUnitShouldListener : ShouldListener
    {
        public override void LogError(string message)
        {
            Assert.Fail(message);
        }

        public override void LogPending(string message)
        {
            Assert.Ignore(message);
        }
    }
}