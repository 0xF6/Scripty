namespace Scripty.Objects
{
    using Interfaces;

    public class Boolean : IHashable
    {
        public bool Value { get; set; }

        public string Type() => ObjectType.BooleanObj;

        public string Inspect() => Value.ToString().ToLower();

        #region Implementation of IHashable

        public HashKey HashKey() => new() {Type = Type(), Value = Value ? 1 : 0};

        #endregion
    }
}