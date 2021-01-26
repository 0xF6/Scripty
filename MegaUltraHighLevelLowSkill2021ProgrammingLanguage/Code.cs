using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Code : INode
    {
        public List<IStatement> Statements { get; set; }

        public string TokenLiteral() => this.Statements.Count > 0 ? this.Statements.First().TokenLiteral() : "";


        public string Str()
        {
            return this.Statements.Aggregate("", (current, statement) => $"{current}{statement.Str()}");
        }
    }
}