using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scripty;
using Scripty.Literals;
using Scripty.Statements;

namespace ScriptyTests
{
    public class FunctionLiteralTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FunctionLiteralTest()
        {
            const string input = "fun (x, y) { x + y; }";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            Assert.AreEqual("ExpressionStatement", program.Statements.First().GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={program.Statements.First().GetType().Name}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("FunctionLiteral", stmt.Expression.GetType().Name,
                $"first statement not of type 'FunctionLiteral', got={stmt.Expression.GetType().Name}");

            var function = stmt.Expression as FunctionLiteral;

            Assert.AreEqual(2, function.Parameters.Count);

            Assert.AreEqual("x", function.Parameters.First().Value);
            Assert.AreEqual("y", function.Parameters.Last().Value);

            Assert.AreEqual(1, function.Body.Statements.Count);

            Assert.AreEqual("ExpressionStatement", function.Body.Statements.First().GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={function.Body.Statements.First().GetType().Name}");

            var bodyStmt = function.Body.Statements.First() as ExpressionStatement;

            Assert.AreEqual("(x + y)", bodyStmt.Str());
        }

        [Test]
        public void FunctionParameterParsingTest()
        {
            var tests = new FunctionTests[]
            {
                new() {Input = "fun() {}", ExpectedParams = new List<string>()},
                new() {Input = "fun(x) {}", ExpectedParams = new List<string> {"x"}},
                new() {Input = "fun(x, y, z) {}", ExpectedParams = new List<string> {"x", "y", "z"}}
            };

            foreach (var functionTests in tests)
            {
                var lexer = new Lexer(functionTests.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);

                Assert.AreEqual("ExpressionStatement", program.Statements.First().GetType().Name,
                    $"first statement not of type 'ExpressionStatement', got={program.Statements.First().GetType().Name}");

                var stmt = program.Statements.First() as ExpressionStatement;

                Assert.AreEqual("FunctionLiteral", stmt.Expression.GetType().Name,
                    $"first statement not of type 'FunctionLiteral', got={stmt.Expression.GetType().Name}");

                var function = stmt.Expression as FunctionLiteral;

                Assert.AreEqual(functionTests.ExpectedParams.Count, function.Parameters.Count);
                for (var i = 0; i < functionTests.ExpectedParams.Count; i++)
                {
                    var functionTestsExpectedParam = functionTests.ExpectedParams[i];
                    Assert.AreEqual(functionTestsExpectedParam, function.Parameters[i].Value);
                }
            }
        }
    }
}