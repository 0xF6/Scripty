using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Null : IObject
    {
        public string Type()
        {
            return ObjectType.NullObj;
        }

        public string Inspect()
        {
            return "null";
        }
    }
}