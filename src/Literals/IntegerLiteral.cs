namespace Scripty.Literals
{
    using System;
    using Interfaces;

    public class IntegerLiteral : IExpression
    {
        public Token Token { get; set; }
        public long Value { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => TokenLiteral();

        public void ExpressionNode() => throw new NotImplementedException();

        public static implicit operator IntegerLiteral(long v) => new()
            {Token = new Token {Type = Token.Int, Literal = v.ToString()}, Value = v};
    }
}