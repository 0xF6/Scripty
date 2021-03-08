namespace ScriptyTests
{
    using NUnit.Framework;
    using Scripty.Objects;

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
                new() {Input = "5 + true;", Expected = "[SC3] type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "5 + true; 12;", Expected = "[SC3] type mismatch: INTEGER + BOOLEAN"},
                new() {Input = "-true;", Expected = "[SC4] unknown operator: -BOOLEAN"},
                new() {Input = "false + true;", Expected = "[SC2] unknown operator: BOOLEAN + BOOLEAN"},
                new()
                {
                    Input = "5; true + false; 6;",
                    Expected = "[SC2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "if (10 > 1) { true + false; }",
                    Expected = "[SC2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "if (10 > 1) { if (9 > 1) { return true + true; } return false; }",
                    Expected = "[SC2] unknown operator: BOOLEAN + BOOLEAN"
                },
                new()
                {
                    Input = "kekpuk",
                    Expected = "[SC5] identifier not found: kekpuk"
                },
                new()
                {
                    Input = "'foo' - 'bar'",
                    Expected = "[SC2] unknown operator: STRING - STRING"
                }
            };

            foreach (var errorTest in tests)
            {
                var evaluated = StaticTests.TestEval(errorTest.Input);

                Assert.AreEqual(nameof(ScriptyError), evaluated.GetType().Name);

                var errObj = (ScriptyError) evaluated;

                Assert.AreEqual(errorTest.Expected, errObj.Message);
            }
        }
    }
}