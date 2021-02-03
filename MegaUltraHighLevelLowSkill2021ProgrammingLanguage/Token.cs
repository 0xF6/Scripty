using System.Collections.Generic;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage
{
    public class Token
    {
        public const string Illegal = "ILLEGAL";
        public const string Eof = "EOF";

        // identifiers
        public const string Ident = "IDENT";
        public const string Int = "INT";

        //operators
        public const string Assign = "=";
        public const string Plus = "+";
        public const string Minus = "-";
        public const string Bang = "!";
        public const string Slash = "/";
        public const string Asterisk = "*";
        public const string Gt = ">";
        public const string Lt = "<";
        public const string Eq = "==";
        public const string NotEq = "!=";

        // delimiters
        public const string Comma = ",";
        public const string Semicolon = ";";
        public const string Lparen = "(";
        public const string Rparen = ")";
        public const string Lbrace = "{";
        public const string Rbrace = "}";

        // keywords
        public const string Function = "FUNCTION";
        public const string Let = "LET";
        public const string True = "TRUE";
        public const string False = "FALSE";
        public const string If = "IF";
        public const string Else = "ELSE";
        public const string Return = "RETURN";

        private readonly Dictionary<string, string> keywords = new()
        {
            {"fun", Function},
            {"let", Let},
            {"true", True},
            {"false", False},
            {"if", If},
            {"else", Else},
            {"return", Return}
        };

        public string Type { get; set; }
        public string Literal { get; set; }

        public string LookUpIdent(string ident)
        {
            if (keywords.TryGetValue(ident, out var tok)) return tok;
            return Ident;
        }
    }
}