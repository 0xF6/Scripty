namespace Scripty
{
    public class Lexer
    {
        public Lexer(string input)
        {
            Input = input;
            Position = 0;
            ReadPosition = 0;
            ReadChar();
        }

        private string Input { get; }
        private int Position { get; set; }
        private int ReadPosition { get; set; }
        private char Ch { get; set; }

        public Token NextToken()
        {
            SkipWhitespace();
            var tok = new Token();
            switch (Ch)
            {
                case '=':
                    if (PeekChar() == '=')
                    {
                        var ch = Ch;
                        ReadChar();
                        var literal = $"{ch}{Ch}";
                        tok = NewToken(Token.Eq, literal);
                    }
                    else
                    {
                        tok = NewToken(Token.Assign, Ch.ToString());
                    }

                    break;
                case '!':
                    if (PeekChar() == '=')
                    {
                        var ch = Ch;
                        ReadChar();
                        var literal = $"{ch}{Ch}";
                        tok = NewToken(Token.NotEq, literal);
                    }
                    else
                    {
                        tok = NewToken(Token.Bang, Ch.ToString());
                    }

                    break;
                case '-':
                    tok = NewToken(Token.Minus, Ch.ToString());
                    break;
                case '/':
                    tok = NewToken(Token.Slash, Ch.ToString());
                    break;
                case '*':
                    tok = NewToken(Token.Asterisk, Ch.ToString());
                    break;
                case '<':
                    tok = NewToken(Token.Lt, Ch.ToString());
                    break;
                case '>':
                    tok = NewToken(Token.Gt, Ch.ToString());
                    break;
                case ';':
                    tok = NewToken(Token.Semicolon, Ch.ToString());
                    break;
                case '(':
                    tok = NewToken(Token.Lparen, Ch.ToString());
                    break;
                case ')':
                    tok = NewToken(Token.Rparen, Ch.ToString());
                    break;
                case ',':
                    tok = NewToken(Token.Comma, Ch.ToString());
                    break;
                case '+':
                    tok = NewToken(Token.Plus, Ch.ToString());
                    break;
                case '{':
                    tok = NewToken(Token.Lbrace, Ch.ToString());
                    break;
                case '}':
                    tok = NewToken(Token.Rbrace, Ch.ToString());
                    break;
                case '"':
                    tok.Type = Token.String;
                    tok.Literal = ReadString(false);
                    break;
                case '\'':
                    tok.Type = Token.String;
                    tok.Literal = ReadString(true);
                    break;
                case '[':
                    tok = NewToken(Token.Lbracket, Ch.ToString());
                    break;
                case ']':
                    tok = NewToken(Token.Rbracket, Ch.ToString());
                    break;
                case ':':
                    tok = NewToken(Token.Colon, Ch.ToString());
                    break;
                case '\0':
                    tok.Type = Token.Eof;
                    tok.Literal = "";
                    break;
                default:
                    if (IsLetter(Ch))
                    {
                        tok.Literal = ReadIdentifier();
                        tok.Type = tok.LookUpIdent(tok.Literal);
                        return tok;
                    }
                    else if (IsDigit(Ch))
                    {
                        tok.Type = Token.Int;
                        tok.Literal = ReadNumber();
                        return tok;
                    }
                    else
                    {
                        tok = NewToken(Token.Illegal, Ch.ToString());
                    }

                    break;
            }

            ReadChar();
            return tok;
        }

        private string ReadString(bool singleQuote)
        {
            var position = Position + 1;
            while (true)
            {
                ReadChar();
                if (singleQuote)
                {
                    if (Ch == '\'' || Ch == '\0') break;
                }
                else
                {
                    if (Ch == '"' || Ch == '\0') break;
                }
            }

            return Input[position..Position];
        }

        private char PeekChar() => ReadPosition >= Input.Length ? '\0' : Input[ReadPosition];


        private void ReadChar()
        {
            Ch = ReadPosition >= Input.Length ? '\0' : Input[ReadPosition];
            Position = ReadPosition;
            ReadPosition += 1;
        }

        private string ReadIdentifier()
        {
            var position = Position;
            while (IsLetter(Ch)) ReadChar();
            return Input[position..Position];
        }

        private string ReadNumber()
        {
            var position = Position;
            while (IsDigit(Ch)) ReadChar();
            return Input[position..Position];
        }

        private void SkipWhitespace()
        {
            while (Ch == ' ' || Ch == '\n' || Ch == '\t' || Ch == '\r') ReadChar();
        }

        private static Token NewToken(string type, string ch) => new() {Type = type, Literal = ch};

        private static bool IsLetter(char ch) => 'a' <= ch && ch <= 'z' || 'A' <= ch && ch <= 'Z';

        private static bool IsDigit(char ch) => '0' <= ch && ch <= '9';
    }
}