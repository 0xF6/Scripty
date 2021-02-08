namespace ScriptyTests
{
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Expressions;
    using Scripty.Statements;

    public class PrefixExpressionsParseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PrefixExpressionsParseTest()
        {
            var prefixTests = new PrefixTest[]
            {
                new() {Input = "!5", Operator = "!", IntegerValue = 5},
                new() {Input = "-50", Operator = "-", IntegerValue = 50}
            };

            foreach (var prefixTest in prefixTests)
            {
                var lexer = new Lexer(prefixTest.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);

                Assert.AreEqual(1, program.Statements.Count,
                    $"program should have 1 statement, got={program.Statements.Count}");

                var stmt = program.Statements.First() as ExpressionStatement;

                Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                    $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

                Assert.AreEqual("PrefixExpression", stmt.Expression.GetType().Name,
                    $"expression not of type 'PrefixExpression', got={stmt.Expression.GetType().Name}");

                var exp = stmt.Expression as PrefixExpression;

                Assert.AreEqual(prefixTest.Operator, exp.Operator,
                    $"operator is not {prefixTest.Operator}, got={exp.Operator}");

                StaticTests.TestIntegerLiteral(exp.Right, prefixTest.IntegerValue);
            }
        }
    }
}