using System;
using System.Collections.Generic;
using Scripty.Interfaces;
using Scripty.Objects;

namespace Scripty.BuiltinFunctions
{
    public static class Puts
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            foreach (var o in args) Console.WriteLine(o.Inspect());

            return Evaluator.ScriptyNull;
        }
    }
}