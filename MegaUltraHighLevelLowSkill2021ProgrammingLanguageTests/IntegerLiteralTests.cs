using System;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
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
            CheckParserErrors(parser);
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

        private void CheckParserErrors(Parser parser)
        {
            var errors = parser.Errors;
            Assert.AreEqual(0, errors.Count);
        }
    }
}