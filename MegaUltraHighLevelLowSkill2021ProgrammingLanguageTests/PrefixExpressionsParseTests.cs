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
    struct PrefixTest
    {
        public string Input { get; set; }
        public string Operator { get; set; }
        public long IntegerValue { get; set; }
    }

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
                new PrefixTest {Input = "!5", Operator = "!", IntegerValue = 5},
                new PrefixTest {Input = "-50", Operator = "-", IntegerValue = 50}
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