using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
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