namespace ScriptyTests
{
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Statements;

    public class IdentifierExpressionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IdentifierExpressionTest()
        {
            const string input = "fooBar;";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);
            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

            StaticTests.TestIdentifier(stmt.Expression, "fooBar");
        }
    }
}