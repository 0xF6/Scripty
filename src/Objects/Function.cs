namespace Scripty.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using Expressions;
    using Interfaces;
    using Statements;

    public class Function : IObject
    {
        public List<Identifier> Parameters { get; set; }
        public BlockStatement Body { get; set; }
        public Environment Environment { get; set; }

        public string Type() => ObjectType.FunctionObj;

        public string Inspect() =>
            $"fun({string.Join(", ", Parameters.Select(identifier => identifier.Str()).ToList())}){{\n{Body.Str()}\n}}";
    }
}