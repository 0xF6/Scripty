using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class FunctionObjectTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FunctionObjectTest()
        {
            const string input = "fun(x) { x + 2; };";
            var evaluated = StaticTests.TestEval(input);

            Assert.AreEqual(nameof(Function), evaluated.GetType().Name);

            var fn = (Function) evaluated;

            Assert.AreEqual(1, fn.Parameters.Count);

            Assert.AreEqual("x", fn.Parameters.First().Str());

            const string expectedBody = "(x + 2)";

            Assert.AreEqual(expectedBody, fn.Body.Str());
        }


        [Test]
        public void FunctionApplicationTest()
        {
            var tests = new LetStatementWithIntegerValueEvalTestCase[]
            {
                new() {Input = "let id = fun(x) { x; }; id(5);", Expected = 5},
                new() {Input = "let id = fun(x) { return x; }; id(5);", Expected = 5},
                new() {Input = "let double = fun(x) { x * 2; }; double(5);", Expected = 10},
                new() {Input = "let add = fun(x, y) { x + y; }; add(228, 322)", Expected = 550},
                new() {Input = "let add = fun(x, y) { x + y; }; add(5 + 5, add(5, 5));", Expected = 20},
                new() {Input = "fun(x) { x; }(5);", Expected = 5},
            };

            foreach (var functionTests in tests)
            {
                var evaluated = StaticTests.TestEval(functionTests.Input);
                StaticTests.TestIntegerObject(evaluated, functionTests.Expected);
            }
        }


        [Test]
        public void ClosureTest()
        {
            const string input = @"
let newAdder = fun(x) {
  fun(y) {x + y; };
};

let addTwo = newAdder(2);
addTwo(2);";

            var evaluated = StaticTests.TestEval(input);
            StaticTests.TestIntegerObject(evaluated, 4);
        }

        [Test]
        public void RecursionTest()
        {
            const string input = @"
let recursion = fun(x) {
  if(x > 100) {
    return true;
  } else {
    recursion(x + 1);
  }
};

recursion(0)";

            var evaluated = StaticTests.TestEval(input);
            StaticTests.TestBooleanObject(evaluated, true);
        }
    }
}