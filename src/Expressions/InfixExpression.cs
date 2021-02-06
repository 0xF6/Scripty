using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions
{
    public class InfixExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Left { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return $"({Left.Str()} {Operator} {Right.Str()})";
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}