using System.Linq;
using NUnit.Framework;
using Scripty;
using Scripty.Literals;
using Scripty.Statements;

namespace ScriptyTests
{
    public class IntegerLiteralTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IntegerLiteralTest()
        {
            const string input = "100500;";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            StaticTests.CheckParserErrors(parser);
            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

            Assert.AreEqual("IntegerLiteral", stmt.Expression.GetType().Name,
                $"expression not of type 'IntegerLiteral', got={stmt.Expression.GetType().Name}");

            var int64Literal = stmt.Expression as IntegerLiteral;

            Assert.AreEqual(100500, int64Literal.Value, $"literal not 100500, got={int64Literal.Value}");

            Assert.AreEqual("100500", int64Literal.TokenLiteral(),
                $"literals token literal not 100500, got={int64Literal.TokenLiteral()}");
        }
    }
}