namespace ScriptyTests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Scripty.Interfaces;
    using Scripty.Objects;

    public class BuiltinTests
    {
        private const string MapReduceAndSum = @"let map = fun(arr, f) {
    let iter = fun(arr, acc) {
        if (length(arr) == 0) {
            acc
        } else {
            iter(rest(arr), push(acc, f(first(arr))));
        }
    }; 
    iter(arr, []);
};

let reduce = fun(arr, initial, f) {
    let iter = fun(arr, result) {
        if (length(arr) == 0) {
            result
        } else {
            iter(rest(arr), f(result, first(arr)));
        }
    };

    iter(arr, initial);
};

let sum = fun(arr) {
    reduce(arr, 0, fun(initial, el) { initial + el });
};";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BuiltinTest1()
        {
            var input = $"{MapReduceAndSum} sum([1,2,3,4]);";

            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(Integer), evaluated.GetType().Name);

            var integer = (Integer) evaluated;

            Assert.AreEqual(10, integer.Value);
        }

        [Test]
        public void BuiltinTest2()
        {
            var input = $@"{MapReduceAndSum} 
let a = fun(x) {{ x + 2 }};

map([1,2,3,4], a);";

            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(Array), evaluated.GetType().Name);

            var arr = (Array) evaluated;
            var expected = new List<IObject>
            {
                (Integer) 3,
                (Integer) 4,
                (Integer) 5,
                (Integer) 6
            };
            for (var i = 0; i < expected.Count; i++)
            {
                var ex = (Integer) expected[i];
                var evaled = (Integer) arr.Elements[i];
                Assert.AreEqual(ex.Value, evaled.Value);
            }
        }
    }
}