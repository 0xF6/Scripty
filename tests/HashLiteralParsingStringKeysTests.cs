namespace ScriptyTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Literals;
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

            var expected = new Dictionary<string, long>()
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

            var tests = new Dictionary<string, ExpressionTestDelegate>()
            {
                {
                    "one",
                    (expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 0, "+", (IntegerLiteral) 1))
                },
                {
                    "two",
                    (expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 228, "-", (IntegerLiteral) 226))
                },
                {
                    "three",
                    (expression =>
                        StaticTests.TestInfixExpression(expression, (IntegerLiteral) 45, "/", (IntegerLiteral) 15))
                },
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
    }
}