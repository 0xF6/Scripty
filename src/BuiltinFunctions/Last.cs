namespace Scripty.BuiltinFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public static class Last
    {
        public static Builtin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new Error(7, (Integer) 1, args.Count.ToString(), null);
            Integer idx;
            if (args.First().Type() == ObjectType.ArrayObj)
            {
                var arr = (Array) args.First();

                idx = (long) arr.Elements.Count - 1;
                return arr.Elements.Count > 0 ? arr.Elements.Last() : new Error(10, arr, null, idx);
            }

            if (args.First().Type() != ObjectType.StringObj) return new Error(9, args.First(), null, null);
            var str = (String) args.First();
            idx = (long) str.Value.Length - 1;
            if (str.Value.Length <= 0) return new Error(10, str, null, idx);
            String s = str.Value[^1].ToString();

            return s;
        }
    }
}