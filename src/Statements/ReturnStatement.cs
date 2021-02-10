using System;
using Scripty.Interfaces;

namespace Scripty.Statements
{
    public class ReturnStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression ReturnValue { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str()
        {
            var res = $"{TokenLiteral()} ";
            if (!(ReturnValue is null)) res = $"{res}{ReturnValue.Str()}";
            res = $"{res};";
            return res;
        }

        public void StatementNode() => throw new NotImplementedException();
    }
}