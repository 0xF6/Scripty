namespace Scripty.Literals
{
    using System;
    using Interfaces;

    public class StringLiteral : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => TokenLiteral();

        public void ExpressionNode() => throw new NotImplementedException();
    }
}