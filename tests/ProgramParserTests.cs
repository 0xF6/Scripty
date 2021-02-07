using System.Collections.Generic;
using NUnit.Framework;
using Scripty;
using Scripty.Expressions;
using Scripty.Interfaces;
using Scripty.Statements;

namespace ScriptyTests
{
    public class ProgramParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ProgramParserTest()
        {
            var program = new Code
            {
                Statements = new List<IStatement>
                {
                    new LetStatement
                    {
                        Token = new Token {Type = Token.Let, Literal = "let"},
                        Name = new Identifier
                        {
                            Token = new Token {Type = Token.Ident, Literal = "foo"},
                            Value = "foo"
                        },
                        Value = new Identifier
                        {
                            Token = new Token {Type = Token.Ident, Literal = "bar"},
                            Value = "bar"
                        }
                    }
                }
            };
            Assert.AreEqual("let foo = bar;", program.Str(), $"wrong code from AST, got={program.Str()}");
        }
    }
}