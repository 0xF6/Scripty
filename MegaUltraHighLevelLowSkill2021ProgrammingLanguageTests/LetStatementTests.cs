using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class LetStatementTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LetStatementsTest()
        {
            const string input = @"let x = 5;
let y = 10;
let foo = 123456789;";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);

            var program = parser.ParseCode();
            StaticTests.CheckParserErrors(parser);
            Assert.IsNotNull(program, "ParseProgram returned null");
            Assert.AreEqual(program.Statements.Count, 3,
                $"program.Statements does not contain 3 statements, got={program.Statements.Count}");
            var tests = new Identifier[]
            {
                new Identifier {Value = "x"},
                new Identifier {Value = "y"},
                new Identifier {Value = "foo"},
            };

            for (var i = 0; i < tests.Length; i++)
            {
                var stmt = program.Statements[i];
                var tt = tests[i];
                StaticTests.TestLetStatement(stmt as LetStatement, tt.Value);
            }
        }
    }
}