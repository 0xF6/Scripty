using Scripty.Delegates;
using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class Builtin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type()
        {
            return ObjectType.BuiltinObj;
        }

        public string Inspect()
        {
            return "builtin object";
        }
    }
}