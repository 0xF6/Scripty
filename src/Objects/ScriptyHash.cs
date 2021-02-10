namespace Scripty.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class ScriptyHash : IObject
    {
        public Dictionary<HashKey, HashPair> Pairs { get; set; }

        #region Implementation of IObject

        public string Type() => ObjectType.HashObj;

        public string Inspect()
        {
            var pairs = Pairs.Select(pair => $"{pair.Value.Key.Inspect()}: {pair.Value.Value.Inspect()}");
            return $"{{{string.Join(", ", pairs)}}}";
        }

        public Dictionary<string, IObject> Properties { get; set; }

        #endregion
    }
}