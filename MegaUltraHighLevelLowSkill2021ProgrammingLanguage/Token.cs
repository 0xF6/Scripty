using System.Collections.Generic;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    using System.Collections;

    public class Token
    {
        public const string ILLEGAL = "ILLEGAL";
        public const string EOF = "EOF";

        // identifiers
        public const string IDENT = "IDENT";
        public const string INT = "INT";

        //operators
        public const string ASSIGN = "=";
        public const string PLUS = "+";
        public const string MINUS = "-";
        public const string BANG = "!";
        public const string SLASH = "/";
        public const string ASTERISK = "*";
        public const string GT = ">";
        public const string LT = "<";
        public const string EQ = "==";
        public const string NOT_EQ = "!=";

        // delimiters
        public const string COMMA = ",";
        public const string SEMICOLON = ";";
        public const string LPAREN = "(";
        public const string RPAREN = ")";
        public const string LBRACE = "{";
        public const string RBRACE = "}";

        // keywords
        public const string FUNCTION = "FUNCTION";
        public const string LET = "LET";
        public const string TRUE = "TRUE";
        public const string FALSE = "FALSE";
        public const string IF = "IF";
        public const string ELSE = "ELSE";
        public const string RETURN = "RETURN";

        private Dictionary<string, string> keywords = new Dictionary<string, string>()
        {
            {"fn", FUNCTION},
            {"let", LET},
            {"true", TRUE},
            {"false", FALSE},
            {"if", IF},
            {"else", ELSE},
            {"return", RETURN},
        };

        public string Type { get; set; }
        public string Literal { get; set; }

        public string LookUpIdent(string ident)
        {
            if (this.keywords.TryGetValue(ident, out var tok)) return tok;
            return IDENT;
        }
    }
}