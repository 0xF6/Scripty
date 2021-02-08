namespace Scripty.Objects
{
    using Interfaces;

    public class ReturnValue : IObject
    {
        public IObject Value { get; set; }

        public string Type() => ObjectType.ReturnValueObj;

        public string Inspect() => Value.Inspect();
    }
}