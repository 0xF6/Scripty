using System.Collections.Generic;
using NUnit.Framework;
using Scripty.Objects;

namespace ScriptyTests
{
    public class StringHashKeyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StringHashKeyTest1()
        {
            var tests = new Dictionary<ScriptyString, ScriptyString>
            {
                {"Hello", "Hello"},
                {"Diff1", "Diff1"}
            };

            foreach (var (key, value) in tests) Assert.AreEqual(key.HashKey().Value, value.HashKey().Value);
        }
    }
}