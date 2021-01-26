using System;
using System.Collections.Generic;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Delegates;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Enums;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Literals;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Parser
    {
        public Lexer Lexer { get; set; }
        public Token CurrentToken { get; set; }
        public Token PeekToken { get; set; }
        public List<string> Errors { get; set; }

        public Dictionary<string, PrefixParseFn> PrefixParseFns { get; set; }
        public Dictionary<string, InfixParseFn> InfixParseFns { get; set; }
        public Dictionary<string, Precedences> Precedences { get; set; }

        public Parser(Lexer lexer)
        {
            this.SetPrecedences();
            this.SetPrefixFns();
            this.SetInfixFns();
            this.Lexer = lexer;
            this.Errors = new List<string>();
            this.NextToken();
            this.NextToken();
        }

        private void SetPrecedences()
        {
            this.Precedences = new Dictionary<string, Precedences>()
            {
                {Token.EQ, Enums.Precedences.EQUALS},
                {Token.NOT_EQ, Enums.Precedences.EQUALS},
                {Token.LT, Enums.Precedences.LESSGREATER},
                {Token.GT, Enums.Precedences.LESSGREATER},
                {Token.PLUS, Enums.Precedences.SUM},
                {Token.MINUS, Enums.Precedences.SUM},
                {Token.SLASH, Enums.Precedences.PRODUCT},
                {Token.ASTERISK, Enums.Precedences.PRODUCT},
            };
        }

        private void SetPrefixFns()
        {
            this.PrefixParseFns = new Dictionary<string, PrefixParseFn>()
            {
                {Token.IDENT, new PrefixParseFn(this.ParseIdentifier)},
                {Token.INT, new PrefixParseFn(this.ParseIntegerLiteral)},
                {Token.BANG, new PrefixParseFn(this.ParsePrefixExpression)},
                {Token.MINUS, new PrefixParseFn(this.ParsePrefixExpression)},
            };
        }

        private void SetInfixFns()
        {
            this.InfixParseFns = new Dictionary<string, InfixParseFn>()
            {
                {Token.PLUS, this.ParseInfixExpression},
                {Token.MINUS, this.ParseInfixExpression},
                {Token.SLASH, this.ParseInfixExpression},
                {Token.ASTERISK, this.ParseInfixExpression},
                {Token.EQ, this.ParseInfixExpression},
                {Token.NOT_EQ, this.ParseInfixExpression},
                {Token.LT, this.ParseInfixExpression},
                {Token.GT, this.ParseInfixExpression},
            };
        }

        private IExpression ParsePrefixExpression()
        {
            var expression = new PrefixExpression
            {
                Token = this.CurrentToken,
                Operator = this.CurrentToken.Literal
            };
            this.NextToken();
            expression.Right = this.ParseExpression(Enums.Precedences.LOWEST);
            return expression;
        }

        private IExpression ParseIntegerLiteral()
        {
            var lit = new IntegerLiteral {Token = this.CurrentToken};
            var success = long.TryParse(this.CurrentToken.Literal, out var value);
            if (!success)
            {
                this.Errors.Add($"could not parse {this.CurrentToken.Literal} into int64");
                return null;
            }

            lit.Value = value;
            return lit;
        }

        private IExpression ParseIdentifier() => new Identifier
            {Token = this.CurrentToken, Value = this.CurrentToken.Literal};


        private void NoPrefixParseFnError(string t) => this.Errors.Add($"no prefix parse function found for {t}");

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

        private void RegisterPrefixParseFn(string t, PrefixParseFn fn) => this.PrefixParseFns.Add(t, fn);

        private void RegisterInfixParseFn(string t, InfixParseFn fn) => this.InfixParseFns.Add(t, fn);

        private IStatement ParseStatement()
        {
            switch (this.CurrentToken.Type)
            {
                case Token.LET:
                    return this.ParseLetStatement();
                case Token.RETURN:
                    return this.ParseReturnStatement();
                default:
                    return this.ParseExpressionStatement();
            }
        }

        private ExpressionStatement ParseExpressionStatement()
        {
            var stmt = new ExpressionStatement
            {
                Token = this.CurrentToken,
                Expression = this.ParseExpression(Enums.Precedences.LOWEST)
            };
            if (this.PeekTokenIs(Token.SEMICOLON)) this.NextToken();
            return stmt;
        }

        private IExpression ParseExpression(Precedences precedence)
        {
            var valueExists = this.PrefixParseFns.TryGetValue(this.CurrentToken.Type, out var prefix);
            if (!valueExists)
            {
                this.NoPrefixParseFnError(this.CurrentToken.Type);
                return null;
            }

            var leftExp = prefix();
            while (!this.PeekTokenIs(Token.SEMICOLON) && precedence < this.PeekPrecedence())
            {
                var infixExists = this.InfixParseFns.TryGetValue(this.PeekToken.Type, out var infix);
                if (!infixExists) return leftExp;
                this.NextToken();
                leftExp = infix(leftExp);
            }
            return leftExp;
        }

        private Precedences PeekPrecedence() => this.Precedences.TryGetValue(this.PeekToken.Type, out var precedence)
            ? precedence
            : Enums.Precedences.LOWEST;

        private Precedences CurrentPrecedence() =>
            this.Precedences.TryGetValue(this.CurrentToken.Type, out var precedence)
                ? precedence
                : Enums.Precedences.LOWEST;

        private IExpression ParseInfixExpression(IExpression left)
        {
            var expression = new InfixExpression
                {Token = this.CurrentToken, Operator = this.CurrentToken.Literal, Left = left};

            var precedence = this.CurrentPrecedence();
            this.NextToken();
            expression.Right = this.ParseExpression(precedence);

            return expression;
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