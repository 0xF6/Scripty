namespace Scripty.Interfaces
{
    using System.Collections.Generic;

    public interface IObject
    {
        public Dictionary<string, IObject> Properties { get; set; }
        string Type();
        string Inspect();
    }
}