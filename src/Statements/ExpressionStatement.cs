using System;
using Scripty.Interfaces;

namespace Scripty.Statements
{
    public class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Expression { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return !(Expression is null) ? Expression.Str() : "";
        }


        public void StatementNode()
        {
            throw new NotImplementedException();
        }
    }
}