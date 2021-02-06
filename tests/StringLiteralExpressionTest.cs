using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class StringLiteralExpressionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StringLiteralExpressionTest1()
        {
            var tests = new[]
            {
                "'hello'", "\"world\""
            };


            foreach (var test in tests)
            {
                var lexer = new Lexer(test);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);

                Assert.AreEqual(nameof(ExpressionStatement), program.Statements.First().GetType().Name);

                var stmt = (ExpressionStatement) program.Statements.First();

                Assert.AreEqual(nameof(StringLiteral), stmt.Expression.GetType().Name);

                var literal = (StringLiteral) stmt.Expression;

                Assert.AreEqual(test.Replace("'", "").Replace("\"", ""), literal.Value);
            }
        }
    }
}