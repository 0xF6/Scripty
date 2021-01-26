using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions
{
    public class PrefixExpression : IExpression
    {
        public Token Token { get; set; }
        public string Operator { get; set; }
        public IExpression Right { get; set; }
        public string TokenLiteral() => this.Token.Literal;

        public string Str()
        {
            return $"({this.Operator}{this.Right.Str()})";
        }

        public void ExpressionNode()
        {
            throw new System.NotImplementedException();
        }
    }
}