namespace Scripty.Objects
{
    using Delegates;
    using Interfaces;

    public class Builtin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type() => ObjectType.BuiltinObj;

        public string Inspect() => "builtin object";
    }
}