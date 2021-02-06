using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class ArrayLiteralParsingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ArrayLiteralParsingTest()
        {
            const string input = "[1, 2 * 2, 3 + 3]";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);

            Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

            var stmt = (ExpressionStatement) program.Statements.First();

            Assert.AreEqual(nameof(ArrayLiteral), stmt.Expression.GetType().Name);

            var array = (ArrayLiteral) stmt.Expression;

            Assert.AreEqual(3, array.Elements.Count);

            StaticTests.TestIntegerLiteral(array.Elements.First(), 1);
            var int2 = new IntegerLiteral {Token = new Token {Type = Token.Int, Literal = "2"}, Value = 2};
            var int3 = new IntegerLiteral {Token = new Token {Type = Token.Int, Literal = "3"}, Value = 3};
            StaticTests.TestInfixExpression(array.Elements[1], int2, "*", int2);
            StaticTests.TestInfixExpression(array.Elements[2], int3, "+", int3);
        }
    }
}