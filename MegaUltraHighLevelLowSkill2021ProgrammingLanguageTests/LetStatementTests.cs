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
            CheckParserErrors(parser);
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
                TestLetStatement(stmt as LetStatement, tt.Value);
            }
        }

        private void CheckParserErrors(Parser parser)
        {
            var errors = parser.Errors;
            Assert.AreEqual(0, errors.Count);
        }

        private static void TestLetStatement(LetStatement statement, string name)
        {
            Assert.AreEqual(statement.TokenLiteral(), "let",
                $"statement token literal not 'let', got={statement.TokenLiteral()}");
            Assert.AreEqual(statement.GetType().Name, "LetStatement",
                $"statement is not of LetStatement type, got={statement.GetType().Name}");
            Assert.AreEqual(name, statement.Name.Value, $"statement name value not {name}, got={statement.Name.Value}");
            Assert.AreEqual(name, statement.Name.TokenLiteral(),
                $"statement name token literal not {name}, got={statement.Name.TokenLiteral()}");
        }
    }
}