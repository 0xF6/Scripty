namespace Scripty.Objects
{
    using Interfaces;

    public class Integer : IObject
    {
        public long Value { get; set; }

        public string Type() => ObjectType.IntegerObj;

        public string Inspect() => $"{Value}";

        public static implicit operator Integer(long v) => new() {Value = v};
    }
}