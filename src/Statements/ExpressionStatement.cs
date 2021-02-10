namespace Scripty.Statements
{
    using System;
    using Interfaces;

    public class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Expression { get; set; }

        public string TokenLiteral() => Token.Literal;

        public string Str() => !(Expression is null) ? Expression.Str() : "";


        public void StatementNode() => throw new NotImplementedException();
    }
}