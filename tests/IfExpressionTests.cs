namespace ScriptyTests
{
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Expressions;
    using Scripty.Statements;

    public class IfExpressionTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IfExpressionTest()
        {
            const string input = "if (x < y) { x }";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

            Assert.AreEqual("IfExpression", stmt.Expression.GetType().Name);

            var exp = stmt.Expression as IfExpression;

            Assert.AreEqual("(x < y)", exp.Condition.Str());

            Assert.AreEqual(1, exp.Consequence.Statements.Count);

            Assert.AreEqual("ExpressionStatement", exp.Consequence.Statements.First().GetType().Name);

            var consequence = exp.Consequence.Statements.First() as ExpressionStatement;

            StaticTests.TestIdentifier(consequence.Expression, "x");

            Assert.AreEqual(null, exp.Alternative);
        }

        [Test]
        public void IfElseExpressionTest()
        {
            const string input = "if (x < y) { x } else { y }";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();

            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

            Assert.AreEqual("IfExpression", stmt.Expression.GetType().Name);

            var exp = stmt.Expression as IfExpression;

            Assert.AreEqual("(x < y)", exp.Condition.Str());

            Assert.AreEqual(1, exp.Consequence.Statements.Count);

            Assert.AreEqual("ExpressionStatement", exp.Consequence.Statements.First().GetType().Name);

            var consequence = exp.Consequence.Statements.First() as ExpressionStatement;

            StaticTests.TestIdentifier(consequence.Expression, "x");

            Assert.AreEqual(1, exp.Alternative.Statements.Count);

            Assert.AreEqual("ExpressionStatement", exp.Alternative.Statements.First().GetType().Name);

            var alt = exp.Alternative.Statements.First() as ExpressionStatement;

            StaticTests.TestIdentifier(alt.Expression, "y");
        }
    }
}