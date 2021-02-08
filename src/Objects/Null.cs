namespace Scripty.Objects
{
    using Interfaces;

    public class Null : IObject
    {
        public string Type() => ObjectType.NullObj;

        public string Inspect() => "null";
    }
}