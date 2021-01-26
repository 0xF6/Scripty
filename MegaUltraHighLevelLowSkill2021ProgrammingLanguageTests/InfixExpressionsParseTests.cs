using System;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    struct InfixTest
    {
        public string Input { get; set; }
        public long LeftValue { get; set; }
        public long RightValue { get; set; }
        public string Operator { get; set; }
    }

    public class InfixExpressionsParseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InfixExpressionsParseTest()
        {
            var infixTests = new InfixTest[]
            {
                new InfixTest {Input = "5 + 5", LeftValue = 5, RightValue = 5, Operator = "+"},
                new InfixTest {Input = "5 - 5", LeftValue = 5, RightValue = 5, Operator = "-"},
                new InfixTest {Input = "5 * 5", LeftValue = 5, RightValue = 5, Operator = "*"},
                new InfixTest {Input = "5 / 5", LeftValue = 5, RightValue = 5, Operator = "/"},
                new InfixTest {Input = "5 > 5", LeftValue = 5, RightValue = 5, Operator = ">"},
                new InfixTest {Input = "5 < 5", LeftValue = 5, RightValue = 5, Operator = "<"},
                new InfixTest {Input = "5 == 5", LeftValue = 5, RightValue = 5, Operator = "=="},
                new InfixTest {Input = "5 != 5", LeftValue = 5, RightValue = 5, Operator = "!="},
            };

            foreach (var infixTest in infixTests)
            {
                var lexer = new Lexer(infixTest.Input);
                var parser = new Parser(lexer);
                var program = parser.ParseCode();
                StaticTests.CheckParserErrors(parser);

                Assert.AreEqual(1, program.Statements.Count,
                    $"program should have 1 statement, got={program.Statements.Count}");

                var stmt = program.Statements.First() as ExpressionStatement;

                Assert.AreEqual("ExpressionStatement", stmt.GetType().Name,
                    $"first statement not of type 'ExpressionStatement', got={stmt.GetType().Name}");

                var exp = stmt.Expression as InfixExpression;

                StaticTests.TestInfixExpression(stmt.Expression, exp.Left, exp.Operator, exp.Right);
            }
        }
    }
}