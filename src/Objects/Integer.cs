using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class Integer : IObject
    {
        public long Value { get; set; }

        public string Type()
        {
            return ObjectType.IntegerObj;
        }

        public string Inspect()
        {
            return $"{Value}";
        }

        public static implicit operator Integer(long v)
        {
            return new() {Value = v};
        }
    }
}