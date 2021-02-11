namespace Scripty.Objects
{
    using System.Collections.Generic;
    using Interfaces;

    public class ReturnValue : IObject
    {
        public IObject Value { get; set; }

        public string Type() => ObjectType.ReturnValueObj;

        public string Inspect() => Value.Inspect();
        public Dictionary<string, IObject> Properties { get; set; }
    }
}