using System;
using FizzBuzz;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FizzBuzzUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var expected = @"1
2
Fizz
4
Buzz
Fizz
7
8
Fizz
Buzz
11
Fizz
13
14
FizzBuzz
";

            var serviceUnderTest = new FizzBuzzGenerator();
            var actual = serviceUnderTest.Execute();

            Assert.AreEqual(expected, actual);
        }
    }
}
