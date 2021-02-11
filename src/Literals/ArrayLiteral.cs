namespace Scripty.Literals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class ArrayLiteral : IExpression
    {
        public Token Token { get; set; }
        public List<IExpression> Elements { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => $"[{string.Join(", ", Elements.Select(expression => expression.Str()).ToList())}]";

        public void ExpressionNode() => throw new NotImplementedException();
    }
}