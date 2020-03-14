using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

namespace TestProject
{
    public class TestClass
    {
        [Test]
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
        public void Test1()
        {
            const int i = 0;
            Console.WriteLine(i);
            string s = "";
            string a = "1", b = "3";
            Console.WriteLine(s);
            Assert.Pass();
        }

        [Test]
        public void TestEquals()
        {
            Assert.AreEqual("a", Identity("a"));
            Assert.AreEqual("a", Identity("a"), "reason");
            Assert.AreEqual("a", Identity("a"), "Reason: {0}", "reason");
            Assert.That(Identity("a"), Is.EqualTo("a"));
            Assert.That(Identity("a"), Is.Not.EqualTo("b"));
        }

        [Test]
        public void TestThrow()
        {
            Assert.DoesNotThrow(() => { });
            Assert.Throws<ArgumentNullException>(() => throw new ArgumentNullException());
            Assert.That(() => { }, Throws.Nothing);
            Assert.That(() => throw new ArgumentException(), Throws.Exception);
            Assert.That(() => throw new ArgumentNullException(), Throws.Exception.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestRest()
        {
            Assert.Contains("a", new[] {"a", "b"});
            Assert.That(new[] {"a", "b"}, Contains.Item("a"));
            Assert.That(new[] {"a", "b"}, Does.Contain("b"));
        }

        private static T Identity<T>(T t)
        {
            return t;
        }
    }
}