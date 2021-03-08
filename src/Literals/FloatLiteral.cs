namespace Scripty.Literals
{
    using System;
    using Interfaces;

    public class FloatLiteral : IExpression
    {
        public Token Token { get; set; }
        public double Value { get; set; }


        public string TokenLiteral() => Token.Literal;

        public string Str() => TokenLiteral();

        public void ExpressionNode() => throw new NotImplementedException();

        public static implicit operator FloatLiteral(double v) => new()
            {Token = new Token {Type = Token.Int, Literal = v.ToString()}, Value = v};
    }
}