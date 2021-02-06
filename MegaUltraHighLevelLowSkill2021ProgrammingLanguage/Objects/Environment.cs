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
        }

        public IObject? Get(string key)
        {
            return Store.GetValueOrDefault(key, null);
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