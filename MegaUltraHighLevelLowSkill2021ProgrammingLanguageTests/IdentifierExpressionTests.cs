using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
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
            CheckParserErrors(parser);
            Assert.AreEqual(1, program.Statements.Count,
                $"program should have 1 statement, got={program.Statements.Count}");

            var stmt = program.Statements.First() as ExpressionStatement;

            Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

            Assert.AreEqual("Identifier", stmt.Expression.GetType().Name,
                $"expression not of type 'Identifier', got={stmt.Expression.GetType().Name}");

            var ident = stmt.Expression as Identifier;

            Assert.AreEqual("fooBar", ident.Value, $"identifier not {input}, got={ident.Value}");

            Assert.AreEqual("fooBar", ident.TokenLiteral(),
                $"identifier token literal not {input}, got={ident.TokenLiteral()}");
        }

        private void CheckParserErrors(Parser parser)
        {
            var errors = parser.Errors;
            Assert.AreEqual(0, errors.Count);
        }
    }
}