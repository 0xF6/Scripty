using Scripty.Objects;

namespace Scripty.Interfaces
{
    public interface IHashable : IObject
    {
        HashKey HashKey();
    }
}