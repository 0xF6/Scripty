namespace Scripty.Objects
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class ScriptyInteger : IHashable
    {
        public long Value { get; set; }

        public string Type() => ObjectType.IntegerObj;

        public string Inspect() => $"{Value}";
        public Dictionary<string, IObject> Properties { get; set; }

        #region Implementation of IHashable

        public HashKey HashKey() => new() {Type = Type(), Value = (ulong) Value};

        #endregion

        public static implicit operator ScriptyInteger(long v) => new() {Value = v};
        public static implicit operator ScriptyInteger(ScriptyFloat v) => new() {Value = Convert.ToInt64(v.Value)};
    }
}