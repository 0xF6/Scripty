using System;
using System.Collections.Generic;
using System.Linq;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.BuiltinFunctions
{
    public class Puts
    {
        public static Builtin Build()
        {
            return new() {Fn = Fn};
        }

        private static IObject Fn(List<IObject> args)
        {
            foreach (var o in args)
            {
                Console.WriteLine(o.Inspect());
            }

            return Evaluator.Null;
        }
    }
}