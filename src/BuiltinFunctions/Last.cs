namespace Scripty.BuiltinFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public static class Last
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new ScriptyError(7, (ScriptyInteger) 1, args.Count.ToString(), null);
            ScriptyInteger idx;
            if (args.First().Type() == ObjectType.ArrayObj)
            {
                var arr = (ScriptyArray) args.First();

                idx = (long) arr.Elements.Count - 1;
                return arr.Elements.Count > 0 ? arr.Elements.Last() : new ScriptyError(10, arr, null, idx);
            }

            if (args.First().Type() != ObjectType.StringObj) return new ScriptyError(9, args.First(), null, null);
            var str = (ScriptyString) args.First();
            idx = (long) str.Value.Length - 1;
            if (str.Value.Length <= 0) return new ScriptyError(10, str, null, idx);
            ScriptyString s = str.Value[^1].ToString();

            return s;
        }
    }
}