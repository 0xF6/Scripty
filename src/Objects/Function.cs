using System.Collections.Generic;
using System.Linq;
using Scripty.Expressions;
using Scripty.Interfaces;
using Scripty.Statements;

namespace Scripty.Objects
{
    public class Function : IObject
    {
        public List<Identifier> Parameters { get; set; }
        public BlockStatement Body { get; set; }
        public Environment Environment { get; set; }

        public string Type()
        {
            return ObjectType.FunctionObj;
        }

        public string Inspect()
        {
            return
                $"fun({string.Join(", ", Parameters.Select(identifier => identifier.Str()).ToList())}){{\n{Body.Str()}\n}}";
        }
    }
}