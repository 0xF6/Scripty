namespace Scripty.Objects
{
    using System.Collections.Generic;
    using Interfaces;

    public class ScriptyNull : IObject
    {
        public string Type() => ObjectType.NullObj;

        public string Inspect() => "null";
        public Dictionary<string, IObject> Properties { get; set; }
    }
}