namespace Scripty.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class BlockStatement : IStatement
    {
        public Token Token { get; set; }
        public List<IStatement> Statements { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => Statements.Aggregate("", (current, statement) => $"{current}{statement.Str()}");


        public void StatementNode() => throw new NotImplementedException();
    }
}