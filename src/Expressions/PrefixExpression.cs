using System;
using Scripty.Interfaces;

namespace Scripty.Expressions
{
    public class PrefixExpression : IExpression
    {
        public Token Token { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return $"({Operator}{Right.Str()})";
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}