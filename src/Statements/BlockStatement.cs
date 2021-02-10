using System;
using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;

namespace Scripty.Statements
{
    public class BlockStatement : IStatement
    {
        public Token Token { get; set; }
        public List<IStatement> Statements { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => Statements.Aggregate("", (current, statement) => $"{current}{statement.Str()}");


        public void StatementNode() => throw new NotImplementedException();
    }
}