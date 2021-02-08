namespace Scripty.Objects
{
    using Interfaces;

    public class Boolean : IObject
    {
        public bool Value { get; set; }

        public string Type() => ObjectType.BooleanObj;

        public string Inspect() => Value.ToString().ToLower();
    }
}