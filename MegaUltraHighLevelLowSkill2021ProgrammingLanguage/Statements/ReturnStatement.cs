using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements
{
    public class ReturnStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression ReturnValue { get; set; }
        public string TokenLiteral() => this.Token.Literal;

        public string Str()
        {
            var res = $"{this.TokenLiteral()} ";
            if (!(this.ReturnValue is null)) res = $"{res}{this.ReturnValue.Str()}";
            res = $"{res};";
            return res;
        }

        public void StatementNode()
        {
            throw new System.NotImplementedException();
        }
    }
}