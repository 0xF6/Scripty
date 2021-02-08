namespace Scripty.Objects
{
    using Interfaces;

    public class String : IObject
    {
        public string Value { get; set; }

        public string Type() => ObjectType.StringObj;

        public string Inspect() => Value;

        public static implicit operator String(string v) => new() {Value = v};
    }
}