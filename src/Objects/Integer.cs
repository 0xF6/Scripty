namespace Scripty.Objects
{
    using Interfaces;

    public class Integer : IHashable
    {
        public long Value { get; set; }

        public string Type() => ObjectType.IntegerObj;

        public string Inspect() => $"{Value}";

        #region Implementation of IHashable

        public HashKey HashKey() => new() {Type = Type(), Value = (ulong) Value};

        #endregion

        public static implicit operator Integer(long v) => new() {Value = v};
    }
}