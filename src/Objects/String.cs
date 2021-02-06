using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class String : IObject
    {
        public string Value { get; set; }

        public string Type()
        {
            return ObjectType.StringObj;
        }

        public string Inspect()
        {
            return Value;
        }
    }
}