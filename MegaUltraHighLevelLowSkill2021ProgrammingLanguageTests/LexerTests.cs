using System;
using System.Diagnostics;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
{
    public class LexerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LexerTest1()
        {
            const string input = "=;+(){},;!*<>/-5";
            var tests = new Token[]
            {
                new Token {Type = Token.ASSIGN, Literal = "="},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.PLUS, Literal = "+"},
                new Token {Type = Token.LPAREN, Literal = "("},
                new Token {Type = Token.RPAREN, Literal = ")"},
                new Token {Type = Token.LBRACE, Literal = "{"},
                new Token {Type = Token.RBRACE, Literal = "}"},
                new Token {Type = Token.COMMA, Literal = ","},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.BANG, Literal = "!"},
                new Token {Type = Token.ASTERISK, Literal = "*"},
                new Token {Type = Token.LT, Literal = "<"},
                new Token {Type = Token.GT, Literal = ">"},
                new Token {Type = Token.SLASH, Literal = "/"},
                new Token {Type = Token.MINUS, Literal = "-"},
                new Token {Type = Token.INT, Literal = "5"},
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
        public void LexerTest2()
        {
            const string input =
                @"let five = 501;
let ten = 10;
let add = fn (x, y) {
  x + y; 
}; 

let result = add(five, ten); 

if (5 < 10) {
  return true;
} else {
  return false;
};";
            var tests = new Token[]
            {
                new Token {Type = Token.LET, Literal = "let"},
                new Token {Type = Token.IDENT, Literal = "five"},
                new Token {Type = Token.ASSIGN, Literal = "="},
                new Token {Type = Token.INT, Literal = "501"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.LET, Literal = "let"},
                new Token {Type = Token.IDENT, Literal = "ten"},
                new Token {Type = Token.ASSIGN, Literal = "="},
                new Token {Type = Token.INT, Literal = "10"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.LET, Literal = "let"},
                new Token {Type = Token.IDENT, Literal = "add"},
                new Token {Type = Token.ASSIGN, Literal = "="},
                new Token {Type = Token.FUNCTION, Literal = "fn"},
                new Token {Type = Token.LPAREN, Literal = "("},
                new Token {Type = Token.IDENT, Literal = "x"},
                new Token {Type = Token.COMMA, Literal = ","},
                new Token {Type = Token.IDENT, Literal = "y"},
                new Token {Type = Token.RPAREN, Literal = ")"},
                new Token {Type = Token.LBRACE, Literal = "{"},
                new Token {Type = Token.IDENT, Literal = "x"},
                new Token {Type = Token.PLUS, Literal = "+"},
                new Token {Type = Token.IDENT, Literal = "y"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.RBRACE, Literal = "}"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.LET, Literal = "let"},
                new Token {Type = Token.IDENT, Literal = "result"},
                new Token {Type = Token.ASSIGN, Literal = "="},
                new Token {Type = Token.IDENT, Literal = "add"},
                new Token {Type = Token.LPAREN, Literal = "("},
                new Token {Type = Token.IDENT, Literal = "five"},
                new Token {Type = Token.COMMA, Literal = ","},
                new Token {Type = Token.IDENT, Literal = "ten"},
                new Token {Type = Token.RPAREN, Literal = ")"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.IF, Literal = "if"},
                new Token {Type = Token.LPAREN, Literal = "("},
                new Token {Type = Token.INT, Literal = "5"},
                new Token {Type = Token.LT, Literal = "<"},
                new Token {Type = Token.INT, Literal = "10"},
                new Token {Type = Token.RPAREN, Literal = ")"},
                new Token {Type = Token.LBRACE, Literal = "{"},
                new Token {Type = Token.RETURN, Literal = "return"},
                new Token {Type = Token.TRUE, Literal = "true"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.RBRACE, Literal = "}"},
                new Token {Type = Token.ELSE, Literal = "else"},
                new Token {Type = Token.LBRACE, Literal = "{"},
                new Token {Type = Token.RETURN, Literal = "return"},
                new Token {Type = Token.FALSE, Literal = "false"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.RBRACE, Literal = "}"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.EOF, Literal = ""},
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
        public void LexerTest3()
        {
            const string input =
                @"10 == 10;
10 != 5;";
            var tests = new Token[]
            {
                new Token {Type = Token.INT, Literal = "10"},
                new Token {Type = Token.EQ, Literal = "=="},
                new Token {Type = Token.INT, Literal = "10"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.INT, Literal = "10"},
                new Token {Type = Token.NOT_EQ, Literal = "!="},
                new Token {Type = Token.INT, Literal = "5"},
                new Token {Type = Token.SEMICOLON, Literal = ";"},
                new Token {Type = Token.EOF, Literal = ""},
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