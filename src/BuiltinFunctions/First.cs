using System.Collections.Generic;
using System.Linq;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public static class First
    {
        public static Builtin Build() 
              => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new Error(7, (Integer) 1, args.Count.ToString(), null);
            Integer idx = (long) 0;
            if (args.First().Type() == ObjectType.ArrayObj)
            {
                var arr = (Array) args.First();

                return arr.Elements.Count > 0 ? arr.Elements.First() : new Error(10, arr, null, idx);
            }

            if (args.First().Type() != ObjectType.StringObj) return new Error(9, args.First(), null, null);
            var str = (String) args.First();

            if (str.Value.Length <= 0) return new Error(10, str, null, idx);
            var s = str.Value[0].ToString();

            return s;
        }
    }
}
