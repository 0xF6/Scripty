using System.Linq;
using NUnit.Framework;
using Scripty;
using Scripty.Expressions;
using Scripty.Literals;
using Scripty.Statements;

namespace ScriptyTests
{
    public class IndexExpressionParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IndexExpressionParsingTest()
        {
            var input = "someList[1 + 1]";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(IndexExpression), stmt.Expression.GetType().Name);

            var indexExp = (IndexExpression) stmt.Expression;

            StaticTests.TestIdentifier(indexExp.Left, "someList");

            var int1 = new IntegerLiteral {Token = new Token {Type = Token.Int, Literal = "1"}, Value = 1};

            StaticTests.TestInfixExpression(indexExp.Index, int1, "+", int1);
        }
    }
}