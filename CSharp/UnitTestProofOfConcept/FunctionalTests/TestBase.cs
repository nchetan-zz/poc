using NUnit.Framework;

namespace FunctionalTests
{
    [TestFixture]
    public abstract class TestBase
    {
        // 
        // This is the method that drives all unit testing. Test framework will start execution from this method
        // Every derived test class is going to be executed inside this context.
        //
        [SetUp]
        public void TestMethod()
        {
            AdjustContext();
            SetupStubs();
            Execute();
        }

        [TearDown]
        public void TearDown()
        {
            Cleanup();
        }

        protected virtual void Cleanup()
        {
            // Default is no op.
        }

        protected abstract void Execute();

        protected virtual void SetupStubs()
        {
            // Default is no op
        }

        protected virtual void AdjustContext()
        {
            // Default is no op.
        }
    }
}
