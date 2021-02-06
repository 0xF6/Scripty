using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Expressions;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Statements;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Function : IObject
    {
        public List<Identifier> Parameters { get; set; }
        public BlockStatement Body { get; set; }
        public Environment Environment { get; set; }

        public string Type()
        {
            return ObjectType.FunctionObj;
        }

        public string Inspect()
        {
            return
                $"fun({string.Join(", ", Parameters.Select(identifier => identifier.Str()).ToList())}){{\n{Body.Str()}\n}}";
        }
    }
}