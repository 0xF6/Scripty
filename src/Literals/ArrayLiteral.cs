using System;
using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;

namespace Scripty.Literals
{
    public class ArrayLiteral : IExpression
    {
        public Token Token { get; set; }
        public List<IExpression> Elements { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return $"[{string.Join(", ", Elements.Select(expression => expression.Str()).ToList())}]";
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}