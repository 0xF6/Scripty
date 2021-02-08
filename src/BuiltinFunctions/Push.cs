namespace Scripty.BuiltinFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public static class Push
    {
        public static Builtin Build() => new() {Fn = Fn};


        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 2) return new Error(7, (Integer) 2, args.Count.ToString(), null);

            if (args.First().Type() != ObjectType.ArrayObj) return new Error(12, args.First(), "push", null);

            var arr = (Array) args.First();

            var newArrElements = arr.Elements.ToList();

            newArrElements.Add(args[1]);

            return new Array {Elements = newArrElements};
        }
    }
}