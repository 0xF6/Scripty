using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Boolean : IObject
    {
        public bool Value { get; set; }

        public string Type()
        {
            return ObjectType.BooleanObj;
        }

        public string Inspect()
        {
            return Value.ToString().ToLower();
        }
    }
}