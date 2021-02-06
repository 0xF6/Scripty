using System;
using System.Collections.Generic;
using MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Interfaces;

namespace MegaUltraHighLevelLowSkill2021ProgrammingLanguage.Objects
{
    public class Environment
    {
        private readonly Dictionary<string, IObject> Store;

        public Environment()
        {
            Store = new Dictionary<string, IObject>();
            Outer = null;
        }

        public Environment(Environment outer)
        {
            Store = new Dictionary<string, IObject>();
            Outer = outer;
        }

        public Environment? Outer { get; set; }

        public IObject? Get(string key)
        {
            var existingValue = Store.GetValueOrDefault(key, null);
            if (existingValue is null && !(Outer is null))
                existingValue = Outer.Get(key);
            return existingValue;
        }

        public IObject Set(string key, IObject obj)
        {
            Store.Add(key, obj);
            return obj;
        }

        public void Traverse()
        {
            foreach (var storeKey in Store.Keys) Console.WriteLine($"{storeKey} - {Store[storeKey].Type()}");
        }
    }
}