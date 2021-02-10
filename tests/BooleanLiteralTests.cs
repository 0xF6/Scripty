using System.Linq;
using NUnit.Framework;
using Scripty;
using Scripty.Literals;
using Scripty.Statements;

namespace ScriptyTests
{
    public class BooleanLiteralTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BooleanLiteralTest()
        {
            var tests = new BooleanTest[]
            {
                new() {Input = "true", Value = true},
                new() {Input = "false", Value = false}
            };

            foreach (var booleanTest in tests)
            {
                var lexer = new Lexer(booleanTest.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);

                Assert.AreEqual(1, program.Statements.Count,
                    $"program should have 1 statement, got={program.Statements.Count}");


                var stmt = program.Statements.First() as ExpressionStatement;

                Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                    $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

                Assert.AreEqual("BooleanLiteral", stmt.Expression.GetType().Name,
                    $"expression not of type 'BooleanLiteral', got={stmt.Expression.GetType().Name}");

                var booleanLiteral = stmt.Expression as BooleanLiteral;

                Assert.AreEqual(booleanTest.Value, booleanLiteral.Value,
                    $"literal not {booleanTest.Value}, got={booleanLiteral.Value}");

                Assert.AreEqual(booleanTest.Input, booleanLiteral.TokenLiteral(),
                    $"literals token literal not {booleanTest.Input}, got={booleanLiteral.TokenLiteral()}");
            }
        }
    }
}