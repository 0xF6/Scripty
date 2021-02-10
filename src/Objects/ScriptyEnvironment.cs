namespace Scripty.Objects
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class ScriptyEnvironment
    {
        private readonly Dictionary<string, IObject> Store;

        public ScriptyEnvironment()
        {
            Store = new Dictionary<string, IObject>();
            Outer = null;
        }

        public ScriptyEnvironment(ScriptyEnvironment outer)
        {
            Store = new Dictionary<string, IObject>();
            Outer = outer;
        }

        private ScriptyEnvironment Outer { get; }

        public IObject Get(string key)
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