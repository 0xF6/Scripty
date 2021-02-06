using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Delegates;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Builtin : IObject
    {
        public BuiltinFunction Fn { get; set; }

        public string Type()
        {
            return ObjectType.BuiltinObj;
        }

        public string Inspect()
        {
            return "builtin object";
        }
    }
}