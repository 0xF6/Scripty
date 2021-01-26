using System.Collections.Generic;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Parser
    {
        public Lexer Lexer { get; set; }
        public Token CurrentToken { get; set; }
        public Token PeekToken { get; set; }
        public List<string> Errors { get; set; }

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;
            this.Errors = new List<string>();
            this.NextToken();
            this.NextToken();
        }

        public Code ParseCode()
        {
            var program = new Code {Statements = new List<IStatement>()};
            while (this.CurrentToken.Type != Token.EOF)
            {
                var stmt = this.ParseStatement();
                if (!(stmt is null))
                {
                    program.Statements.Add(stmt);
                }

                this.NextToken();
            }

            return program;
        }

        private IStatement ParseStatement()
        {
            switch (this.CurrentToken.Type)
            {
                case Token.LET:
                    return this.ParseLetStatement();
                case Token.RETURN:
                    return this.ParseReturnStatement();
                default:
                    return null;
            }
        }

        private ReturnStatement ParseReturnStatement()
        {
            var stmt = new ReturnStatement {Token = this.CurrentToken};
            this.NextToken();
            // TODO skip expressions until semicolon is encountered
            while (!this.CurTokenIs(Token.SEMICOLON)) this.NextToken();
            return stmt;
        }

        private LetStatement ParseLetStatement()
        {
            var stmt = new LetStatement {Token = this.CurrentToken};
            if (!this.ExpectPeek(Token.IDENT)) return null;


            stmt.Name = new Identifier {Token = this.CurrentToken, Value = this.CurrentToken.Literal};
            if (!this.ExpectPeek(Token.ASSIGN)) return null;


            // TODO skip expressions until semicolon is encountered
            while (!this.CurTokenIs(Token.SEMICOLON)) this.NextToken();
            return stmt;
        }

        private bool CurTokenIs(string t) => this.CurrentToken.Type == t;

        private bool ExpectPeek(string t)
        {
            if (this.PeekTokenIs(t))
            {
                this.NextToken();
                return true;
            }

            this.PeekError(t);
            return false;
        }

        private bool PeekTokenIs(string t) => this.PeekToken.Type == t;

        private void NextToken()
        {
            this.CurrentToken = this.PeekToken;
            this.PeekToken = this.Lexer.NextToken();
        }

        private void PeekError(string t)
        {
            this.Errors.Add($"expected next token to be '{t}', got '{this.PeekToken.Type}' instead");
        }
    }
}