using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions
{
    public class Identifier : IExpression
    {
        public Token Token { get; set; }
        public string Value { get; set; }
        public string TokenLiteral() => this.Token.Literal;

        public string Str() => this.Value;

        public void ExpressionNode()
        {
            throw new System.NotImplementedException();
        }
    }
}