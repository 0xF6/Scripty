using System;
using Scripty.Interfaces;

namespace Scripty.Expressions
{
    public class Identifier : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return Value;
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}