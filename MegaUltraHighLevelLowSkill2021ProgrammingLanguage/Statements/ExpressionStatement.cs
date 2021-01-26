using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements
{
    public class ExpressionStatement : IStatement
    {
        public Token Token { get; set; }
        public IExpression Expression { get; set; }

        public string TokenLiteral() => this.Token.Literal;

        public string Str() => !(this.Expression is null) ? this.Expression.Str() : "";


        public void StatementNode()
        {
            throw new System.NotImplementedException();
        }
    }
}