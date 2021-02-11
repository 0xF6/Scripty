namespace Scripty
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class Code : INode
    {
        public List<IStatement> Statements { get; set; }

        public string TokenLiteral() => Statements.Count > 0 ? Statements.First().TokenLiteral() : "";


        public string Str() =>
            Statements.Aggregate("",
                (current, statement) => $"{current}{statement.Str()}"); // TODO: fix precedences
    }
}