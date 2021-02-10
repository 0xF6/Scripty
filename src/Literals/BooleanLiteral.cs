using System;
using Scripty.Interfaces;

namespace Scripty.Literals
{
    public class BooleanLiteral : IExpression
    {
        public Token Token { get; set; }
        public bool Value { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => TokenLiteral();

        public void ExpressionNode() => throw new NotImplementedException();
    }
}