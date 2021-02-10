namespace Scripty.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class CallExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Function { get; set; }
        public List<IExpression> Arguments { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str()
        {
            var args = Arguments.Select(expression => expression.Str()).ToList();

            return $"{Function.Str()}({string.Join(", ", args)})";
        }

        public void ExpressionNode() => throw new NotImplementedException();
    }
}