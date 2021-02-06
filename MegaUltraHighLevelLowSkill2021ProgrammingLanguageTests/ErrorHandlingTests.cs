using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class ErrorHandlingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ErrorHandlingTest()
        {
            var tests = new OperatorTest[]
            {
                new() {Input = "5 + true;", Expected = "[MUHL3] type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "5 + true; 12;", Expected = "[MUHL3] type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "-true;", Expected = "[MUHL4] unknown operator: -BOOLEAN"},
                new() {Input = "false + true;", Expected = "[MUHL2] unknown operator: BOOLEAN + BOOLEAN"},
                new()
                {
                    Input = "5; true + false; 6;",
                    Expected = "[MUHL2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "if (10 > 1) { true + false; }",
                    Expected = "[MUHL2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "if (10 > 1) { if (9 > 1) { return true + true; } return false; }",
                    Expected = "[MUHL2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "kekpuk",
                    Expected = "[MUHL5] identifier not found: kekpuk"
                }
            };

            foreach (var errorTest in tests)
            {
                var evaluated = StaticTests.TestEval(errorTest.Input);

                Assert.AreEqual(nameof(Error), evaluated.GetType().Name);

                var errObj = (Error) evaluated;

                Assert.AreEqual(errorTest.Expected, errObj.Message);
            }
        }
    }
}