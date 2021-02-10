using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public static class Push
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};


        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 2) return new ScriptyError(7, (ScriptyInteger) 2, args.Count.ToString(), null);

            if (args.First().Type() != ObjectType.ArrayObj) return new ScriptyError(12, args.First(), "push", null);

            var arr = (ScriptyArray) args.First();

            var newArrElements = arr.Elements.ToList();

            newArrElements.Add(args[1]);

            return new ScriptyArray {Elements = newArrElements};
        }
    }
}