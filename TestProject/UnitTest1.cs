using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

namespace TestProject
{
    public class TestClass
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [SuppressMessage("ReSharper", "SuggestVarOrType_BuiltInTypes")]
        [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
        public void Test1()
        {
            const int i = 0;
            Console.WriteLine(i);
            string s = "";
            Console.WriteLine(s);
            Assert.Pass();
        }

        /// <summary>
        ///     Test description
        /// </summary>
        [Test]
        public void Test2()
        {
            Assert.Pass();
        }

        /// Test description
        [Test]
        public void Test3()
        {
            Assert.Pass();
        }

        // Test description
        [Test]
        public void Test4()
        {
            Assert.Pass();
        }

        [Test]
        // Test description
        public void Test5()
        {
            Assert.Pass();
        }
    }
}