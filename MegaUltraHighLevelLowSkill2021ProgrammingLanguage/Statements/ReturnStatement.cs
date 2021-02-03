using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements
{
    public class ReturnStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression? ReturnValue { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            var res = $"{TokenLiteral()} ";
            if (!(ReturnValue is null)) res = $"{res}{ReturnValue.Str()}";
            res = $"{res};";
            return res;
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }
    }
}