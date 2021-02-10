using Scripty.Delegates;
using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class ScriptyBuiltin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type() => ObjectType.BuiltinObj;

        public string Inspect() => "builtin object";
    }
}