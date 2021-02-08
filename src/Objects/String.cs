namespace Scripty.Objects
{
    using Interfaces;

    public class String : IHashable
    {
        public string Value { get; set; }

        public string Type() => ObjectType.StringObj;

        public string Inspect() => Value;

        public static implicit operator String(string v) => new() {Value = v};

        #region Implementation of IHashable

        public HashKey HashKey() => new() {Type = Type(), Value = StringHashCode(Value)};

        private static unsafe ulong StringHashCode(string s)
        {
            fixed (char* chPtr1 = s)
            {
                var num1 = 0x1505UL;
                var num2 = num1;
                var chPtr2 = chPtr1;
                ulong num3;
                while ((num3 = *chPtr2) != 0)
                {
                    num1 = (num1 << 5) + num1 ^ num3;
                    var num4 = (ulong) chPtr2[1];
                    if (num4 == 0) break;
                    num2 = (num2 << 5) + num2 ^ num4;
                    chPtr2 += 2;
                }

                return num1 + num2 * 0x5d588b65UL;
            }
        }

        #endregion
    }
}