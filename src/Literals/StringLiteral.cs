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


        public static implicit operator StringLiteral(string v) =>
            new() {Token = new Token {Type = Token.String, Literal = v}, Value = v};
    }
}