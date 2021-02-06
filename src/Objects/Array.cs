using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Array : IObject
    {
        public List<IObject> Elements { get; set; }

        public string Type()
        {
            return ObjectType.ArrayObj;
        }

        public string Inspect()
        {
            return $"[{string.Join(", ", Elements.Select(element => element.Inspect()).ToList())}]";
        }
    }
}