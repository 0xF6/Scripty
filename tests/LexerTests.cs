using NUnit.Framework;
using Scripty;

namespace ScriptyTests
{
    public class LexerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SymbolTests()
        {
            const string input = "=;+(){},;!*<>/-5";
            var tests = new Token[]
            {
                new() {Type = Token.Assign, Literal = "="},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Plus, Literal = "+"},
                new() {Type = Token.Lparen, Literal = "("},
                new() {Type = Token.Rparen, Literal = ")"},
                new() {Type = Token.Lbrace, Literal = "{"},
                new() {Type = Token.Rbrace, Literal = "}"},
                new() {Type = Token.Comma, Literal = ","},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Bang, Literal = "!"},
                new() {Type = Token.Asterisk, Literal = "*"},
                new() {Type = Token.Lt, Literal = "<"},
                new() {Type = Token.Gt, Literal = ">"},
                new() {Type = Token.Slash, Literal = "/"},
                new() {Type = Token.Minus, Literal = "-"},
                new() {Type = Token.Int, Literal = "5"}
            };

            var l = new Lexer(input);
            foreach (var tt in tests)
            {
                var tok = l.NextToken();
                Assert.AreEqual(tok.Type, tt.Type);
                Assert.AreEqual(tok.Literal, tt.Literal);
            }
        }

        [Test]
        public void KeywordsTest()
        {
            const string input =
                @"let five = 501;
let ten = 10;
let add = fun (x, y) {
  x + y; 
}; 

let result = add(five, ten); 

if (5 < 10) {
  return true;
} else {
  return false;
};
10 == 10;
10 != 9; 
'fooBar'
'foo bar'
[1, 2];
{'kek': 'puk'};
";
            var tests = new Token[]
            {
                new() {Type = Token.Let, Literal = "let"},
                new() {Type = Token.Ident, Literal = "five"},
                new() {Type = Token.Assign, Literal = "="},
                new() {Type = Token.Int, Literal = "501"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Let, Literal = "let"},
                new() {Type = Token.Ident, Literal = "ten"},
                new() {Type = Token.Assign, Literal = "="},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Let, Literal = "let"},
                new() {Type = Token.Ident, Literal = "add"},
                new() {Type = Token.Assign, Literal = "="},
                new() {Type = Token.Function, Literal = "fun"},
                new() {Type = Token.Lparen, Literal = "("},
                new() {Type = Token.Ident, Literal = "x"},
                new() {Type = Token.Comma, Literal = ","},
                new() {Type = Token.Ident, Literal = "y"},
                new() {Type = Token.Rparen, Literal = ")"},
                new() {Type = Token.Lbrace, Literal = "{"},
                new() {Type = Token.Ident, Literal = "x"},
                new() {Type = Token.Plus, Literal = "+"},
                new() {Type = Token.Ident, Literal = "y"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Rbrace, Literal = "}"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Let, Literal = "let"},
                new() {Type = Token.Ident, Literal = "result"},
                new() {Type = Token.Assign, Literal = "="},
                new() {Type = Token.Ident, Literal = "add"},
                new() {Type = Token.Lparen, Literal = "("},
                new() {Type = Token.Ident, Literal = "five"},
                new() {Type = Token.Comma, Literal = ","},
                new() {Type = Token.Ident, Literal = "ten"},
                new() {Type = Token.Rparen, Literal = ")"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.If, Literal = "if"},
                new() {Type = Token.Lparen, Literal = "("},
                new() {Type = Token.Int, Literal = "5"},
                new() {Type = Token.Lt, Literal = "<"},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Rparen, Literal = ")"},
                new() {Type = Token.Lbrace, Literal = "{"},
                new() {Type = Token.Return, Literal = "return"},
                new() {Type = Token.True, Literal = "true"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Rbrace, Literal = "}"},
                new() {Type = Token.Else, Literal = "else"},
                new() {Type = Token.Lbrace, Literal = "{"},
                new() {Type = Token.Return, Literal = "return"},
                new() {Type = Token.False, Literal = "false"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Rbrace, Literal = "}"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Eq, Literal = "=="},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.NotEq, Literal = "!="},
                new() {Type = Token.Int, Literal = "9"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.String, Literal = "fooBar"},
                new() {Type = Token.String, Literal = "foo bar"},
                new() {Type = Token.Lbracket, Literal = "["},
                new() {Type = Token.Int, Literal = "1"},
                new() {Type = Token.Comma, Literal = ","},
                new() {Type = Token.Int, Literal = "2"},
                new() {Type = Token.Rbracket, Literal = "]"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Lbrace, Literal = "{"},
                new() {Type = Token.String, Literal = "kek"},
                new() {Type = Token.Colon, Literal = ":"},
                new() {Type = Token.String, Literal = "puk"},
                new() {Type = Token.Rbrace, Literal = "}"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Eof, Literal = ""}
            };

            var l = new Lexer(input);

            foreach (var tt in tests)
            {
                var tok = l.NextToken();
                Assert.AreEqual(tt.Type, tok.Type,
                    $"test failed: wrong token type - expected='{tt.Type}', got='{tok.Type}'");
                Assert.AreEqual(tt.Literal, tok.Literal,
                    $"test failed: wrong token Literal - expected='{tt.Literal}', got='{tok.Literal}'");
            }
        }

        [Test]
        public void EqualityOperatorsTest()
        {
            const string input =
                @"10 == 10;
10 != 5;";
            var tests = new Token[]
            {
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Eq, Literal = "=="},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Int, Literal = "10"},
                new() {Type = Token.NotEq, Literal = "!="},
                new() {Type = Token.Int, Literal = "5"},
                new() {Type = Token.Semicolon, Literal = ";"},
                new() {Type = Token.Eof, Literal = ""}
            };

            var l = new Lexer(input);

            foreach (var tt in tests)
            {
                var tok = l.NextToken();
                Assert.AreEqual(tt.Type, tok.Type,
                    $"test failed: wrong token type - expected='{tt.Type}', got='{tok.Type}'");
                Assert.AreEqual(tt.Literal, tok.Literal,
                    $"test failed: wrong token Literal - expected='{tt.Literal}', got='{tok.Literal}'");
            }
        }
    }
}