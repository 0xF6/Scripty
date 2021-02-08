namespace Scripty.BuiltinFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public static class Length
    {
        public static Builtin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new Error(7, (Integer) 1, args.Count.ToString(), null);

            return args.First().Type() switch
            {
                ObjectType.StringObj => new Integer {Value = ((String) args.First()).Value.Length},
                ObjectType.ArrayObj => new Integer {Value = ((Array) args.First()).Elements.Count},
                _ => new Error(8, null, args.First().Type(), null)
            };
        }
    }
}