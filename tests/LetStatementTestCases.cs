namespace ScriptyTests
{
    internal struct LetStatementTestCases
    {
        public string Input { get; set; }
        public string ExpectedIdentifier { get; set; }
        private object _expectedVal;

        public LetStatementTestCases ExpectedValueSet<T>(T val)
        {
            _expectedVal = val;
            return this;
        }

        public object ExpectedValueGet()
        {
            return _expectedVal;
        }
    }
}