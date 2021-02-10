using System;
using Scripty.Interfaces;

namespace Scripty.Statements
{
    public class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Expression { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => !(Expression is null) ? Expression.Str() : "";


        public void StatementNode() => throw new NotImplementedException();
    }
}