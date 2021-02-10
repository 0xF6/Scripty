using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class ScriptyArray : IObject
    {
        public List<IObject> Elements { get; set; }

        public string Type() => ObjectType.ArrayObj;

        public string Inspect() => $"[{string.Join(", ", Elements.Select(element => element.Inspect()).ToList())}]";
    }
}