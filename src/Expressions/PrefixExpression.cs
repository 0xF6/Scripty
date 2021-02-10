using System;
using Scripty.Interfaces;

namespace Scripty.Expressions
{
    public class PrefixExpression : IExpression
    {
        public Token Token { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => $"({Operator}{Right.Str()})";

        public void ExpressionNode() => throw new NotImplementedException();
    }
}