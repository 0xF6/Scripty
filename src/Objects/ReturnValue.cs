using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class ReturnValue : IObject
    {
        public IObject Value { get; set; }

        public string Type()
        {
            return ObjectType.ReturnValueObj;
        }

        public string Inspect()
        {
            return Value.Inspect();
        }
    }
}