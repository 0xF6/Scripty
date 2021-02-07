using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;

namespace Scripty
{
    public class Code : INode
    {
        public List<IStatement> Statements { get; set; }

        public string TokenLiteral()
        {
            return Statements.Count > 0 ? Statements.First().TokenLiteral() : "";
        }


        public string Str()
        {
            return Statements.Aggregate("",
                (current, statement) => $"{current}{statement.Str()}"); // TODO: fix precedences
        }
    }
}