namespace Scripty.Literals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class HashLiteral : IExpression
    {
        public Token Token { get; set; }
        public Dictionary<IExpression, IExpression> Pairs { get; set; }

        #region Implementation of INode

        public string TokenLiteral() => Token.Literal;

        public string Str()
        {
            var pairs = (from pairsKey in Pairs.Keys
                let val = Pairs[pairsKey]
                select $"{pairsKey.Str()}:{val.Str()}").ToList();
            return $"{{{string.Join(", ", pairs)}}}";
        }

        public void ExpressionNode() => throw new NotImplementedException();

        #endregion
    }
}