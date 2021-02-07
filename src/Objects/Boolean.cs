using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class Boolean : IObject
    {
        public bool Value { get; set; }

        public string Type()
        {
            return ObjectType.BooleanObj;
        }

        public string Inspect()
        {
            return Value.ToString().ToLower();
        }
    }
}