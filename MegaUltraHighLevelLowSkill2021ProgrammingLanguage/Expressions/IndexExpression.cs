using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions
{
    public class IndexExpression : IExpression
    {
        public Token Token { get; set; }
        public IExpression Left { get; set; }
        public IExpression Index { get; set; }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public string Str()
        {
            return $"({Left.Str()}[{Index.Str()}])";
        }

        public void ExpressionNode()
        {
            throw new System.NotImplementedException();
        }
    }
}