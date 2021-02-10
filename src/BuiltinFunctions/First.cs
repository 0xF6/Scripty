using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public static class First
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new ScriptyError(7, (ScriptyInteger) 1, args.Count.ToString(), null);
            ScriptyInteger idx = 0L;
            if (args.First().Type() == ObjectType.ArrayObj)
            {
                var arr = (ScriptyArray) args.First();

                return arr.Elements.Count > 0 ? arr.Elements.First() : new ScriptyError(10, arr, null, idx);
            }

            if (args.First().Type() != ObjectType.StringObj) return new ScriptyError(9, args.First(), null, null);
            var str = (ScriptyString) args.First();

            if (str.Value.Length <= 0) return new ScriptyError(10, str, null, idx);
            return (ScriptyString) str.Value[0].ToString();
        }
    }
}