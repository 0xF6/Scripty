namespace Scripty.Interfaces
{
    using Objects;

    public interface IHashable : IObject
    {
        HashKey HashKey();
    }
}