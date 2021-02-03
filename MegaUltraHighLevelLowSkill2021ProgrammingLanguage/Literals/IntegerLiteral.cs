using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals
{
    public class IntegerLiteral : IExpression
    {
        public Token Token { get; set; }
        public long Value { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return TokenLiteral();
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}