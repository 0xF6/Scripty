namespace Scripty.Objects
{
    using Interfaces;

    public class ScriptyNull : IObject
    {
        public string Type() => ObjectType.NullObj;

        public string Inspect() => "null";
    }
}