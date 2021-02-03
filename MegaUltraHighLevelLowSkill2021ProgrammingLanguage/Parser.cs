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
        public Parser(Lexer lexer)
        {
            SetPrecedences();
            SetPrefixFns();
            SetInfixFns();
            Lexer = lexer;
            Errors = new List<string>();
            NextToken();
            NextToken();
        }

        public Lexer Lexer { get; set; }
        public Token CurrentToken { get; set; }
        public Token PeekToken { get; set; }
        public List<string> Errors { get; set; }

        public Dictionary<string, PrefixParseFn> PrefixParseFns { get; set; }
        public Dictionary<string, InfixParseFn> InfixParseFns { get; set; }
        public Dictionary<string, Precedences> Precedences { get; set; }

        private void SetPrecedences()
        {
            Precedences = new Dictionary<string, Precedences>
            {
                {Token.Eq, Enums.Precedences.EQUALS},
                {Token.NotEq, Enums.Precedences.EQUALS},
                {Token.Lt, Enums.Precedences.LESSGREATER},
                {Token.Gt, Enums.Precedences.LESSGREATER},
                {Token.Plus, Enums.Precedences.SUM},
                {Token.Minus, Enums.Precedences.SUM},
                {Token.Slash, Enums.Precedences.PRODUCT},
                {Token.Asterisk, Enums.Precedences.PRODUCT},
                {Token.Lparen, Enums.Precedences.CALL}
            };
        }

        private void SetPrefixFns()
        {
            PrefixParseFns = new Dictionary<string, PrefixParseFn>
            {
                {Token.Ident, ParseIdentifier},
                {Token.Int, ParseIntegerLiteral},
                {Token.Bang, ParsePrefixExpression},
                {Token.Minus, ParsePrefixExpression},
                {Token.True, ParseBoolean},
                {Token.False, ParseBoolean},
                {Token.Lparen, ParseGroupedExpression},
                {Token.If, ParseIfExpression},
                {Token.Function, ParseFunctionLiteral}
            };
        }

        private IExpression ParseFunctionLiteral()
        {
            var lit = new FunctionLiteral {Token = CurrentToken};
            if (!ExpectPeek(Token.Lparen)) return null;
            lit.Parameters = ParseFunctionParameters();
            if (!ExpectPeek(Token.Lbrace)) return null;
            lit.Body = ParseBlockStatement();
            return lit;
        }

        private List<Identifier> ParseFunctionParameters()
        {
            var identifiers = new List<Identifier>();

            if (PeekTokenIs(Token.Rparen))
            {
                NextToken();
                return identifiers;
            }

            NextToken();

            var ident = new Identifier {Token = CurrentToken, Value = CurrentToken.Literal};
            identifiers.Add(ident);

            while (PeekTokenIs(Token.Comma))
            {
                NextToken();
                NextToken();
                ident = new Identifier {Token = CurrentToken, Value = CurrentToken.Literal};
                identifiers.Add(ident);
            }

            return !ExpectPeek(Token.Rparen) ? null : identifiers;
        }

        private IExpression ParseIfExpression()
        {
            var expression = new IfExpression {Token = CurrentToken};
            if (!ExpectPeek(Token.Lparen)) return null;
            NextToken();
            expression.Condition = ParseExpression(Enums.Precedences.LOWEST);
            if (!ExpectPeek(Token.Rparen)) return null;
            if (!ExpectPeek(Token.Lbrace)) return null;
            expression.Consequence = ParseBlockStatement();

            if (!PeekTokenIs(Token.Else)) return expression;
            NextToken();
            if (!ExpectPeek(Token.Lbrace)) return null;

            expression.Alternative = ParseBlockStatement();

            return expression;
        }

        private BlockStatement ParseBlockStatement()
        {
            var block = new BlockStatement {Token = CurrentToken, Statements = new List<IStatement>()};
            NextToken();
            while (!CurTokenIs(Token.Rbrace) && !CurTokenIs(Token.Eof))
            {
                var stmt = ParseStatement();
                if (!(stmt is null)) block.Statements.Add(stmt);
                NextToken();
            }

            return block;
        }

        private IExpression? ParseGroupedExpression()
        {
            NextToken();
            var exp = ParseExpression(Enums.Precedences.LOWEST);

            return !ExpectPeek(Token.Rparen) ? null : exp;
        }

        private IExpression ParseBoolean()
        {
            return new BooleanLiteral
                {Token = CurrentToken, Value = CurTokenIs(Token.True)};
        }

        private void SetInfixFns()
        {
            InfixParseFns = new Dictionary<string, InfixParseFn>
            {
                {Token.Plus, ParseInfixExpression},
                {Token.Minus, ParseInfixExpression},
                {Token.Slash, ParseInfixExpression},
                {Token.Asterisk, ParseInfixExpression},
                {Token.Eq, ParseInfixExpression},
                {Token.NotEq, ParseInfixExpression},
                {Token.Lt, ParseInfixExpression},
                {Token.Gt, ParseInfixExpression},
                {Token.Lparen, ParseCallExpression}
            };
        }

        private IExpression ParseCallExpression(IExpression v)
        {
            return new CallExpression
                {Token = CurrentToken, Function = v, Arguments = ParseCallArguments()};
        }

        private List<IExpression> ParseCallArguments()
        {
            var args = new List<IExpression>();
            if (PeekTokenIs(Token.Rparen))
            {
                NextToken();
                return args;
            }

            NextToken();
            args.Add(ParseExpression(Enums.Precedences.LOWEST));
            while (PeekTokenIs(Token.Comma))
            {
                NextToken();
                NextToken();
                args.Add(ParseExpression(Enums.Precedences.LOWEST));
            }

            return !ExpectPeek(Token.Rparen) ? null : args;
        }


        private IExpression ParsePrefixExpression()
        {
            var expression = new PrefixExpression
            {
                Token = CurrentToken,
                Operator = CurrentToken.Literal
            };
            NextToken();
            expression.Right = ParseExpression(Enums.Precedences.PREFIX);
            return expression;
        }

        private IExpression ParseIntegerLiteral()
        {
            var lit = new IntegerLiteral {Token = CurrentToken};
            var success = long.TryParse(CurrentToken.Literal, out var value);
            if (!success)
            {
                Errors.Add($"could not parse {CurrentToken.Literal} into int64");
                return null;
            }

            lit.Value = value;
            return lit;
        }

        private IExpression ParseIdentifier()
        {
            return new Identifier
                {Token = CurrentToken, Value = CurrentToken.Literal};
        }


        private void NoPrefixParseFnError(string t)
        {
            Errors.Add($"no prefix parse function found for {t}");
        }

        public Code ParseCode()
        {
            var program = new Code {Statements = new List<IStatement>()};
            while (CurrentToken.Type != Token.Eof)
            {
                var stmt = ParseStatement();
                if (!(stmt is null)) program.Statements.Add(stmt);

                NextToken();
            }

            return program;
        }

        private void RegisterPrefixParseFn(string t, PrefixParseFn fn)
        {
            PrefixParseFns.Add(t, fn);
        }

        private void RegisterInfixParseFn(string t, InfixParseFn fn)
        {
            InfixParseFns.Add(t, fn);
        }

        private IStatement? ParseStatement()
        {
            return CurrentToken.Type switch
            {
                Token.Let => ParseLetStatement(),
                Token.Return => ParseReturnStatement(),
                _ => ParseExpressionStatement()
            };
        }

        private ExpressionStatement ParseExpressionStatement()
        {
            var stmt = new ExpressionStatement
            {
                Token = CurrentToken,
                Expression = ParseExpression(Enums.Precedences.LOWEST)
            };
            if (PeekTokenIs(Token.Semicolon)) NextToken();
            return stmt;
        }

        private IExpression? ParseExpression(Precedences precedence)
        {
            var valueExists = PrefixParseFns.TryGetValue(CurrentToken.Type, out var prefix);
            if (!valueExists)
            {
                NoPrefixParseFnError(CurrentToken.Type);
                return null;
            }

            var leftExp = prefix();
            while (!PeekTokenIs(Token.Semicolon) && precedence < PeekPrecedence())
            {
                var infixExists = InfixParseFns.TryGetValue(PeekToken.Type, out var infix);
                if (!infixExists) return leftExp;
                NextToken();
                leftExp = infix(leftExp);
            }

            return leftExp;
        }

        private Precedences PeekPrecedence()
        {
            return Precedences.TryGetValue(PeekToken.Type, out var precedence)
                ? precedence
                : Enums.Precedences.LOWEST;
        }

        private Precedences CurrentPrecedence()
        {
            return Precedences.TryGetValue(CurrentToken.Type, out var precedence)
                ? precedence
                : Enums.Precedences.LOWEST;
        }

        private IExpression ParseInfixExpression(IExpression left)
        {
            var expression = new InfixExpression
                {Token = CurrentToken, Operator = CurrentToken.Literal, Left = left};

            var precedence = CurrentPrecedence();
            NextToken();
            expression.Right = ParseExpression(precedence);

            return expression;
        }

        private ReturnStatement ParseReturnStatement()
        {
            var stmt = new ReturnStatement {Token = CurrentToken};
            NextToken();
            stmt.ReturnValue = ParseExpression(Enums.Precedences.LOWEST);

            if (PeekTokenIs(Token.Semicolon)) NextToken();

            return stmt;
        }

        private LetStatement ParseLetStatement()
        {
            var stmt = new LetStatement {Token = CurrentToken};
            if (!ExpectPeek(Token.Ident)) return null;


            stmt.Name = new Identifier {Token = CurrentToken, Value = CurrentToken.Literal};
            if (!ExpectPeek(Token.Assign)) return null;

            NextToken();
            stmt.Value = ParseExpression(Enums.Precedences.LOWEST);

            if (PeekTokenIs(Token.Semicolon)) NextToken();

            return stmt;
        }

        private bool CurTokenIs(string t)
        {
            return CurrentToken.Type == t;
        }

        private bool ExpectPeek(string t)
        {
            if (PeekTokenIs(t))
            {
                NextToken();
                return true;
            }

            PeekError(t);
            return false;
        }

        private bool PeekTokenIs(string t)
        {
            return PeekToken.Type == t;
        }

        private void NextToken()
        {
            CurrentToken = PeekToken;
            PeekToken = Lexer.NextToken();
        }

        private void PeekError(string t)
        {
            Errors.Add($"expected next token to be '{t}', got '{PeekToken.Type}' instead");
        }
    }
}