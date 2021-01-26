using System;
using System.Collections.Generic;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;
using NUnit.Framework;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguageTests
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
                Statements = new List<IStatement>()
                {
                    new LetStatement()
                    {
                        Token = new Token {Type = Token.LET, Literal = "let"},
                        Name = new Identifier
                        {
                            Token = new Token {Type = Token.IDENT, Literal = "foo"},
                            Value = "foo"
                        },
                        Value = new Identifier
                        {
                            Token = new Token {Type = Token.IDENT, Literal = "bar"},
                            Value = "bar"
                        }
                    }
                }
            };
            Assert.AreEqual("let foo = bar;", program.Str(), $"wrong code from AST, got={program.Str()}");
        }
    }
}