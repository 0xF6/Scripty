using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class ScriptyNull : IObject
    {
        public string Type() => ObjectType.NullObj;

        public string Inspect() => "null";
    }
}