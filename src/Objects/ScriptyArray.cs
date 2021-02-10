namespace Scripty.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class ScriptyArray : IObject
    {
        public List<IObject> Elements { get; set; }

        public string Type() => ObjectType.ArrayObj;

        public string Inspect() => $"[{string.Join(", ", Elements.Select(element => element.Inspect()).ToList())}]";
    }
}