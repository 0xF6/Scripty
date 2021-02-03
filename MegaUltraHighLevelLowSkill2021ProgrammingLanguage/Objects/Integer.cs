using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Integer : IObject
    {
        public long Value { get; set; }

        public string Type()
        {
            return ObjectType.IntegerObj;
        }

        public string Inspect()
        {
            return $"{Value}";
        }
    }
}