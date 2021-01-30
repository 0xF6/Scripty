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
                {Token.LPAREN, Enums.Precedences.CALL},
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
                {Token.TRUE, new PrefixParseFn(this.ParseBoolean)},
                {Token.FALSE, new PrefixParseFn(this.ParseBoolean)},
                {Token.LPAREN, new PrefixParseFn(this.ParseGroupedExpression)},
                {Token.IF, new PrefixParseFn(this.ParseIfExpression)},
                {Token.FUNCTION, new PrefixParseFn(this.ParseFunctionLiteral)},
            };
        }

        private IExpression ParseFunctionLiteral()
        {
            var lit = new FunctionLiteral {Token = this.CurrentToken};
            if (!this.ExpectPeek(Token.LPAREN)) return null;
            lit.Parameters = this.ParseFunctionParameters();
            if (!this.ExpectPeek(Token.LBRACE)) return null;
            lit.Body = this.ParseBlockStatement();
            return lit;
        }

        private List<Identifier> ParseFunctionParameters()
        {
            var identifiers = new List<Identifier>();

            if (this.PeekTokenIs(Token.RPAREN))
            {
                this.NextToken();
                return identifiers;
            }

            this.NextToken();

            var ident = new Identifier {Token = this.CurrentToken, Value = this.CurrentToken.Literal};
            identifiers.Add(ident);

            while (this.PeekTokenIs(Token.COMMA))
            {
                this.NextToken();
                this.NextToken();
                ident = new Identifier {Token = this.CurrentToken, Value = this.CurrentToken.Literal};
                identifiers.Add(ident);
            }

            return !this.ExpectPeek(Token.RPAREN) ? null : identifiers;
        }

        private IExpression ParseIfExpression()
        {
            var expression = new IfExpression {Token = this.CurrentToken};
            if (!this.ExpectPeek(Token.LPAREN)) return null;
            this.NextToken();
            expression.Condition = this.ParseExpression(Enums.Precedences.LOWEST);
            if (!this.ExpectPeek(Token.RPAREN)) return null;
            if (!this.ExpectPeek(Token.LBRACE)) return null;
            expression.Consequence = this.ParseBlockStatement();

            if (!this.PeekTokenIs(Token.ELSE)) return expression;
            this.NextToken();
            if (!this.ExpectPeek(Token.LBRACE)) return null;

            expression.Alternative = this.ParseBlockStatement();

            return expression;
        }

        private BlockStatement ParseBlockStatement()
        {
            var block = new BlockStatement {Token = this.CurrentToken, Statements = new List<IStatement>()};
            this.NextToken();
            while (!this.CurTokenIs(Token.RBRACE) && !this.CurTokenIs(Token.EOF))
            {
                var stmt = this.ParseStatement();
                if (!(stmt is null)) block.Statements.Add(stmt);
                this.NextToken();
            }

            return block;
        }

        private IExpression ParseGroupedExpression()
        {
            this.NextToken();
            var exp = this.ParseExpression(Enums.Precedences.LOWEST);

            return !this.ExpectPeek(Token.RPAREN) ? null : exp;
        }

        private IExpression ParseBoolean() => new BooleanLiteral
            {Token = this.CurrentToken, Value = this.CurTokenIs(Token.TRUE)};

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
                {Token.LPAREN, this.ParseCallExpression},
            };
        }

        private IExpression ParseCallExpression(IExpression v) => new CallExpression
            {Token = this.CurrentToken, Function = v, Arguments = this.ParseCallArguments()};

        private List<IExpression> ParseCallArguments()
        {
            var args = new List<IExpression>();
            if (this.PeekTokenIs(Token.RPAREN))
            {
                this.NextToken();
                return args;
            }

            this.NextToken();
            args.Add(this.ParseExpression(Enums.Precedences.LOWEST));
            while (this.PeekTokenIs(Token.COMMA))
            {
                this.NextToken();
                this.NextToken();
                args.Add(this.ParseExpression(Enums.Precedences.LOWEST));
            }

            return !this.ExpectPeek(Token.RPAREN) ? null : args;
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
            stmt.ReturnValue = this.ParseExpression(Enums.Precedences.LOWEST);

            if (this.PeekTokenIs(Token.SEMICOLON)) this.NextToken();

            return stmt;
        }

        private LetStatement ParseLetStatement()
        {
            var stmt = new LetStatement {Token = this.CurrentToken};
            if (!this.ExpectPeek(Token.IDENT)) return null;


            stmt.Name = new Identifier {Token = this.CurrentToken, Value = this.CurrentToken.Literal};
            if (!this.ExpectPeek(Token.ASSIGN)) return null;

            this.NextToken();
            stmt.Value = this.ParseExpression(Enums.Precedences.LOWEST);

            if (this.PeekTokenIs(Token.SEMICOLON)) this.NextToken();

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