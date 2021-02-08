namespace ScriptyTests
{
    using System.Linq;
    using NUnit.Framework;
    using Scripty;
    using Scripty.Statements;

    public class ReturnStatementsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReturnStatementTest()
        {
            const string input = @"
return 5;
return 10;
return 228322;";
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseCode();
            Assert.AreEqual(3, program.Statements.Count,
                $"program should contain 3 statements, got={program.Statements.Count}");
            foreach (var programStatement in program.Statements.Cast<ReturnStatement>())
            {
                Assert.AreEqual("ReturnStatement", programStatement.GetType().Name,
                    $"statement not of ReturnStatement type, got={programStatement.GetType().Name}");
                Assert.AreEqual("return", programStatement.TokenLiteral(),
                    $"statement token literal is not return, got={programStatement.TokenLiteral()}");
            }
        }
    }
}