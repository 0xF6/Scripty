using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Error : IObject
    {
        public string Message { get; set; }

        public string Type()
        {
            return ObjectType.ErrorObj;
        }

        public string Inspect()
        {
            return $"ERROR: {Message}";
        }
    }
}