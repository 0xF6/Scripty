namespace Scripty.BuiltinFunctions
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Objects;

    public static class Puts
    {
        public static Builtin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            foreach (var o in args) Console.WriteLine(o.Inspect());

            return Evaluator.Null;
        }
    }
}