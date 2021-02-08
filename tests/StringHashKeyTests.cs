namespace ScriptyTests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Scripty.Objects;

    public class StringHashKeyTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StringHashKeyTest1()
        {
            var tests = new Dictionary<String, String>
            {
                {"Hello", "Hello"},
                {"Diff1", "Diff1"}
            };

            foreach (var (key, value) in tests) Assert.AreEqual(key.HashKey().Value, value.HashKey().Value);
        }
    }
}