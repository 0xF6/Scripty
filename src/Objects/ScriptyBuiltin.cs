namespace Scripty.Objects
{
    using Delegates;
    using Interfaces;

    public class ScriptyBuiltin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type() => ObjectType.BuiltinObj;

        public string Inspect() => "builtin object";
    }
}