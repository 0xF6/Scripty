using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements
{
    public class BlockStatement : IStatement
    {
        public Token Token { get; set; }
        public List<IStatement> Statements { get; set; }
        public string TokenLiteral() => this.Token.Literal;

        public string Str() => this.Statements.Aggregate("", (current, statement) => $"{current}{statement.Str()}");


        public void StatementNode()
        {
            throw new System.NotImplementedException();
        }
    }
}