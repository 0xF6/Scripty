namespace ScriptyTests
{
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Expressions;
    using Scripty.Statements;

    public class CallExpressionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CallExpressionTest()
        {
            const string input = "sub(1, 2 * 3, 4 + 5)";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            Assert.AreEqual("ExpressionStatement", program.Statements.First().GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={program.Statements.First().GetType().Name}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("CallExpression", stmt.Expression.GetType().Name,
                $"first statement not of type 'CallExpression', got={stmt.Expression.GetType().Name}");

            var exp = stmt.Expression as CallExpression;

            StaticTests.TestIdentifier(exp.Function, "sub");

            Assert.AreEqual(3, exp.Arguments.Count);

            Assert.AreEqual("1", exp.Arguments.First().Str());
            Assert.AreEqual("(2 * 3)", exp.Arguments[1].Str());
            Assert.AreEqual("(4 + 5)", exp.Arguments.Last().Str());
        }
    }
}