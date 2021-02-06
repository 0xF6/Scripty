using System;
using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals
{
    public class FunctionLiteral : IExpression
    {
        public Token Token { get; set; }
        public List<Identifier> Parameters { get; set; }
        public BlockStatement Body { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            var parameters = Parameters.Select(identifier => identifier.Str()).ToList();

            return $"{TokenLiteral()}({string.Join(", ", parameters)}){Body.Str()}";
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}