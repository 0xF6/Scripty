namespace Scripty.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Interfaces;

    [DebuggerDisplay("{Inspect()}")]
    public class ScriptyFloat : IHashable
    {
        public double Value { get; set; }
        public Dictionary<string, IObject> Properties { get; set; }
        public string Type() => ObjectType.FloatObj;

        public string Inspect() => $"{Value}";

        public HashKey HashKey() => new() {Type = Type(), Value = (ulong) Value};

        public static implicit operator ScriptyFloat(double v) => new() {Value = v};

        public static implicit operator ScriptyFloat(ScriptyInteger v) => new() {Value = Convert.ToDouble(v.Value)};
    }
}