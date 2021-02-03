using System;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Boolean : IObject, IEquatable<Boolean>
    {
        public bool Value { get; set; }

        public bool Equals(Boolean other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public string Type()
        {
            return ObjectType.BooleanObj;
        }

        public string Inspect()
        {
            return Value.ToString().ToLower();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Boolean) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}