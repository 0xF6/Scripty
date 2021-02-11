namespace Scripty.Objects
{
    using System.Collections.Generic;
    using Interfaces;

    public class ScriptyBoolean : IHashable
    {
        public bool Value { get; set; }

        public string Type() => ObjectType.BooleanObj;

        public string Inspect() => Value.ToString().ToLower();
        public Dictionary<string, IObject> Properties { get; set; }

        #region Implementation of IHashable

        public HashKey HashKey() => new() {Type = Type(), Value = Value ? 1 : 0};

        #endregion
    }
}