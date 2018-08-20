using Artees.Diagnostics.BDD;
using Xunit;

namespace DocCoverTest
{
    internal class NUnitShouldListener : ShouldListener
    {
        public override void LogError(string message)
        {
            Assert.False(true, message);
        }

        public override void LogPending(string message)
        {
            Assert.False(true, message);
        }
    }
}