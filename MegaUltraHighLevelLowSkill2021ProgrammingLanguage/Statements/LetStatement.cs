using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements
{
    public class LetStatement : IStatement
    {
        public Token Token { get; set; }
        public Identifier Name { get; set; }
        public IExpression Value { get; set; }

        public string TokenLiteral() => this.Token.Literal;

        public string Str()
        {
            var res = $"{this.TokenLiteral()} {this.Name.Str()} = ";
            if (!(this.Value is null)) res = $"{res}{this.Value.Str()}";
            res = $"{res};";
            return res;
        }

        public void StatementNode()
        {
            throw new System.NotImplementedException();
        }
    }
}