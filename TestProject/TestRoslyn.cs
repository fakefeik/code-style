using NUnit.Framework;

using NUnitAssert = NUnit.Framework.Assert;

using static NUnit.Framework.Assert;

namespace TestProject
{
    public class TestRoslyn
    {
        [Test]
        public void TestEquals()
        {
            NUnit.Framework.Assert.AreEqual("", "");
            NUnitAssert.AreEqual("a", "a");
            AreEqual("a", "a");

            Assert.AreEqual("a", "b");
        }

        private static class Assert
        {
            public static void AreEqual<T>(T expected, T actual)
            {
            }
        }
    }
}