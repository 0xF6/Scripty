namespace Scripty.Objects
{
    using System.Collections.Generic;
    using Delegates;
    using Interfaces;

    public class ScriptyBuiltin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type() => ObjectType.BuiltinObj;

        public string Inspect() => "builtin object";
        public Dictionary<string, IObject> Properties { get; set; }
    }
}