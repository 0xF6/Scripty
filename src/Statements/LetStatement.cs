using System;
using Scripty.Expressions;
using Scripty.Interfaces;

namespace Scripty.Statements
{
    public class LetStatement : IStatement
    {
        public Token Token { get; set; }
        public Identifier Name { get; set; }
        public IExpression Value { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str()
        {
            var res = $"{TokenLiteral()} {Name.Str()} = ";
            if (!(Value is null)) res = $"{res}{Value.Str()}";
            res = $"{res};";
            return res;
        }

        public void StatementNode() => throw new NotImplementedException();
    }
}