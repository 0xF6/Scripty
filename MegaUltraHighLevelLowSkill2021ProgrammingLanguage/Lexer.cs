using System;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Lexer
    {
        public string Input { get; set; }
        public int Position { get; set; }
        public int ReadPosition { get; set; }
        public char Ch { get; set; }

        public Lexer(string input)
        {
            this.Input = input;
            this.Position = 0;
            this.ReadPosition = 0;
            this.ReadChar();
        }

        public Token NextToken()
        {
            this.SkipWhitespace();
            var tok = new Token { };
            switch (this.Ch)
            {
                case '=':
                    if (this.PeekChar() == '=')
                    {
                        var ch = this.Ch;
                        this.ReadChar();
                        var literal = $"{ch}{this.Ch}";
                        tok = NewToken(Token.EQ, literal);
                    }
                    else tok = NewToken(Token.ASSIGN, this.Ch.ToString());

                    break;
                case '!':
                    if (this.PeekChar() == '=')
                    {
                        var ch = this.Ch;
                        this.ReadChar();
                        var literal = $"{ch}{this.Ch}";
                        tok = NewToken(Token.NOT_EQ, literal);
                    }
                    else tok = NewToken(Token.BANG, this.Ch.ToString());

                    break;
                case '-':
                    tok = NewToken(Token.MINUS, this.Ch.ToString());
                    break;
                case '/':
                    tok = NewToken(Token.SLASH, this.Ch.ToString());
                    break;
                case '*':
                    tok = NewToken(Token.ASTERISK, this.Ch.ToString());
                    break;
                case '<':
                    tok = NewToken(Token.LT, this.Ch.ToString());
                    break;
                case '>':
                    tok = NewToken(Token.GT, this.Ch.ToString());
                    break;
                case ';':
                    tok = NewToken(Token.SEMICOLON, this.Ch.ToString());
                    break;
                case '(':
                    tok = NewToken(Token.LPAREN, this.Ch.ToString());
                    break;
                case ')':
                    tok = NewToken(Token.RPAREN, this.Ch.ToString());
                    break;
                case ',':
                    tok = NewToken(Token.COMMA, this.Ch.ToString());
                    break;
                case '+':
                    tok = NewToken(Token.PLUS, this.Ch.ToString());
                    break;
                case '{':
                    tok = NewToken(Token.LBRACE, this.Ch.ToString());
                    break;
                case '}':
                    tok = NewToken(Token.RBRACE, this.Ch.ToString());
                    break;
                case '\0':
                    tok.Type = Token.EOF;
                    tok.Literal = "";
                    break;
                default:
                    if (IsLetter(this.Ch))
                    {
                        tok.Literal = this.ReadIdentifier();
                        tok.Type = tok.LookUpIdent(tok.Literal);
                        return tok;
                    }
                    else if (IsDigit(this.Ch))
                    {
                        tok.Type = Token.INT;
                        tok.Literal = this.ReadNumber();
                        return tok;
                    }
                    else tok = NewToken(Token.ILLEGAL, this.Ch.ToString());

                    break;
            }

            this.ReadChar();
            return tok;
        }

        private char PeekChar() => this.ReadPosition >= this.Input.Length ? '\0' : this.Input[this.ReadPosition];


        private void ReadChar()
        {
            this.Ch = this.ReadPosition >= this.Input.Length ? '\0' : this.Input[this.ReadPosition];
            this.Position = this.ReadPosition;
            this.ReadPosition += 1;
        }

        private string ReadIdentifier()
        {
            var position = this.Position;
            while (IsLetter(this.Ch)) this.ReadChar();
            return this.Input[position..this.Position];
        }

        private string ReadNumber()
        {
            var position = this.Position;
            while (IsDigit(this.Ch)) this.ReadChar();
            return this.Input[position..this.Position];
        }

        private void SkipWhitespace()
        {
            while (this.Ch == ' ' || this.Ch == '\n' || this.Ch == '\t' || this.Ch == '\r') this.ReadChar();
        }

        private static Token NewToken(string type, string ch) => new Token {Type = type, Literal = ch};
        private static bool IsLetter(char ch) => 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';
        private static bool IsDigit(char ch) => '0' <= ch && ch <= '9';
    }
}