using System;
using Scripty.Interfaces;

namespace Scripty.Literals
{
    public class StringLiteral : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return TokenLiteral();
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}