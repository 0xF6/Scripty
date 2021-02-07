using Scripty.Interfaces;

namespace Scripty.Objects
{
    public class Null : IObject
    {
        public string Type()
        {
            return ObjectType.NullObj;
        }

        public string Inspect()
        {
            return "null";
        }
    }
}