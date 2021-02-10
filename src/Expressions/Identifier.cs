using System;
using Scripty.Interfaces;

namespace Scripty.Expressions
{
    public class Identifier : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => Value;

        public void ExpressionNode() => throw new NotImplementedException();
    }
}