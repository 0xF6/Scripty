namespace ScriptyTests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Literals;
    using Scripty.Objects;
    using Scripty.Statements;

    public class HashLiteralParsingStringKeysTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HashLiteralParsingStringKeysTest()
        {
            const string input = "{'one': 1, 'two': 2, 'three': 3}";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(HashLiteral), stmt.Expression.GetType().Name);

            var hash = (HashLiteral) stmt.Expression;

            Assert.AreEqual(3, hash.Pairs.Count);

            var expected = new Dictionary<string, long>
            {
                {"one", 1},
                {"two", 2},
                {"three", 3}
            };

            foreach (var pairsKey in hash.Pairs.Keys)
            {
                Assert.AreEqual(nameof(StringLiteral), pairsKey.GetType().Name);
                var literal = (StringLiteral) pairsKey;

                var expectedValue = expected[literal.Str()];

                StaticTests.TestIntegerLiteral(hash.Pairs[pairsKey], expectedValue);
            }
        }

        [Test]
        public void EmptyHashLiteralParsingTest()
        {
            const string input = "{}";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(HashLiteral), stmt.Expression.GetType().Name);

            var hash = (HashLiteral) stmt.Expression;

            Assert.AreEqual(0, hash.Pairs.Count);
        }

        [Test]
        public void HashLiteralWithExpressionParsingTest()
        {
            const string input = "{'one': 0 + 1, 'two': 228 - 226, 'three': 45 / 15}";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(HashLiteral), stmt.Expression.GetType().Name);

            var hash = (HashLiteral) stmt.Expression;

            Assert.AreEqual(3, hash.Pairs.Count);

            var tests = new Dictionary<string, ExpressionTestDelegate>
            {
                {
                    "one",
                    expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 0, "+", (IntegerLiteral) 1)
                },
                {
                    "two",
                    expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 228, "-", (IntegerLiteral) 226)
                },
                {
                    "three",
                    expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 45, "/", (IntegerLiteral) 15)
                }
            };

            foreach (var pairsKey in hash.Pairs.Keys)
            {
                var value = hash.Pairs[pairsKey];
                Assert.AreEqual(nameof(StringLiteral), pairsKey.GetType().Name);
                var literal = (StringLiteral) pairsKey;

                var keyExists = tests.TryGetValue(literal.Str(), out var testFunc);

                Assert.True(keyExists);

                testFunc(value);
            }
        }


        [Test]
        public void HashLiteralTest()
        {
            const string input = @"let two = 'two';
{
  'one': 10 -9,
  two: 2,
  'thr' + 'ee': 6/2,
  4: 4,
  true: 5,
  false: 6
};";
            var evaluated = StaticTests.TestEval(input);
            Assert.AreEqual(nameof(ScriptyHash), evaluated.GetType().Name);
            var result = (ScriptyHash) evaluated;
            var expected = new Dictionary<HashKey, int>
            {
                {((ScriptyString) "one").HashKey(), 1},
                {((ScriptyString) "two").HashKey(), 2},
                {((ScriptyString) "three").HashKey(), 3},
                {((ScriptyInteger) 4).HashKey(), 4},
                {Evaluator.True.HashKey(), 5},
                {Evaluator.False.HashKey(), 6}
            };

            Assert.AreEqual(expected.Count, result.Pairs.Count);

            foreach (var (hkey, hval) in expected)
            {
                var pairExists = result.Pairs.TryGetValue(hkey, out var pair);
                Assert.True(pairExists);
                StaticTests.TestIntegerObject(pair.Value, hval);
            }
        }

        [Test]
        public void HashIndexExpressionTest()
        {
            var passingTests = new IntegerTests[]
            {
                new() {Input = "{'a': 5}['a']", Expected = 5},
                new() {Input = "let key = 'k'; {'k': 5}[key]", Expected = 5},
                new() {Input = "{5: 5}[5]", Expected = 5},
                new() {Input = "{true: 5}[true]", Expected = 5},
                new() {Input = "{false: 5}[false]", Expected = 5}
            };

            var failingTests = new OperatorTest[]
            {
                new() {Input = "{}[5]", Expected = "ERROR: [SC16] `{}` does not contain key `5`"},
                new() {Input = "{'a': 5}[5]", Expected = "ERROR: [SC16] `{'a': 5}` does not contain key `5`"}
            };

            foreach (var passingTest in passingTests)
            {
                var evaluated = StaticTests.TestEval(passingTest.Input);
                StaticTests.TestIntegerObject(evaluated, passingTest.Expected);
            }

            foreach (var failingTest in failingTests)
            {
                var evaluated = StaticTests.TestEval(failingTest.Input);
                Assert.AreEqual(failingTest.Expected, evaluated.Inspect());
            }
        }
    }
}