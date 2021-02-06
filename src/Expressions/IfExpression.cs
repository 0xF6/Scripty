using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions
{
    public class IfExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Condition { get; set; }
        public BlockStatement Consequence { get; set; }
        public BlockStatement Alternative { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            var res = $"if{Condition.Str()} {Consequence.Str()}";
            if (!(Alternative is null)) res = $"{res} else {Alternative.Str()}";
            return res;
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}