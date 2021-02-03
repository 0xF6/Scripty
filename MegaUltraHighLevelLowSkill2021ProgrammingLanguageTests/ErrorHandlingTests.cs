using System;
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
                new() {Input = "5 + true;", Expected = "type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "5 + true; 12;", Expected = "type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "-true;", Expected = "unknown operator: -BOOLEAN"},
                new() {Input = "false + true;", Expected = "unknown operator: BOOLEAN + BOOLEAN"},
                new() {Input = "5; true + false; 6;", Expected = "unknown operator: BOOLEAN + BOOLEAN"},
                new() {Input = "if (10 > 1) { true + false; }", Expected = "unknown operator: BOOLEAN + BOOLEAN"},
                new()
                {
                    Input = "if (10 > 1) { if (9 > 1) { return true + true; } return false; }",
                    Expected = "unknown operator: BOOLEAN + BOOLEAN"
                },
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