using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public static class Length
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new ScriptyError(7, (ScriptyInteger) 1, args.Count.ToString(), null);

            return args.First().Type() switch
            {
                ObjectType.StringObj => new ScriptyInteger {Value = ((ScriptyString) args.First()).Value.Length},
                ObjectType.ArrayObj => new ScriptyInteger {Value = ((ScriptyArray) args.First()).Elements.Count},
                _ => new ScriptyError(8, null, args.First().Type(), null)
            };
        }
    }
}