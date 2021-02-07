using System;
using Scripty.Interfaces;

namespace Scripty.Literals
{
    public class BooleanLiteral : IExpression
    {
        public Token Token { get; set; }
        public bool Value { get; set; }

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