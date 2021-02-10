namespace Scripty.BuiltinFunctions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Objects;

    public static class ParseFloat
    {
        public static ScriptyBuiltin Build() => new() {Fn = Fn};

        private static IObject Fn(List<IObject> args)
        {
            if (args.Count != 1) return new ScriptyError(7, (ScriptyInteger) 1, args.Count.ToString(), null);
            var arg = args.First();
            bool valueParsed;
            double parsed;
            switch (arg.Type())
            {
                case ObjectType.IntegerObj:
                    parsed = Convert.ToDouble(((ScriptyInteger) arg).Value);
                    valueParsed = true;
                    break;
                case ObjectType.FloatObj:
                    parsed = ((ScriptyFloat) arg).Value;
                    valueParsed = true;
                    break;
                case ObjectType.StringObj:
                    valueParsed = double.TryParse(((ScriptyString) arg).Value, out parsed);
                    break;
                default:
                    valueParsed = false;
                    parsed = 0;
                    break;
            }

            if (!valueParsed) return new ScriptyError(12, arg, "parseFloat", null);
            return (ScriptyFloat) parsed;
        }
    }
}