namespace Scripty.BuiltinFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public class Rest
    {
        public static Builtin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new Error(7, (Integer) 1, args.Count.ToString(), null);

            if (args.First().Type() != ObjectType.ArrayObj) return new Error(12, args.First(), "rest", null);

            var arr = (Array) args.First();

            var length = arr.Elements.Count;

            if (length > 0)
                return new Array {Elements = arr.Elements.GetRange(1, length - 1).ToList()};

            return new Error(13, arr, "rest", null);
        }
    }
}