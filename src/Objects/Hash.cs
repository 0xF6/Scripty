namespace Scripty.Objects
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class Hash : IObject
    {
        public Dictionary<HashKey, HashPair> Pairs { get; set; }


        #region Implementation of IObject

        public string Type() => ObjectType.HashObj;

        public string Inspect()
        {
            var pairs = Pairs.Select(pair => $"{pair.Value.Key.Inspect()}: {pair.Value.Value.Inspect()}").ToList();
            return $"{{{string.Join(", ", pairs)}}}";
        }

        #endregion
    }
}