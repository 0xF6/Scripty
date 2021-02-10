using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public class Rest
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new ScriptyError(7, (ScriptyInteger) 1, args.Count.ToString(), null);

            if (args.First().Type() != ObjectType.ArrayObj) return new ScriptyError(12, args.First(), "rest", null);

            var arr = (ScriptyArray) args.First();

            var length = arr.Elements.Count;

            if (length > 0)
                return new ScriptyArray {Elements = arr.Elements.GetRange(1, length - 1).ToList()};

            return new ScriptyError(13, arr, "rest", null);
        }
    }
}