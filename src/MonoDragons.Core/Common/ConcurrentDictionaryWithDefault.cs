﻿using System.Collections.Concurrent;

namespace MonoDragons
{
    public class ConcurrentDictionaryWithDefault<Key, Value> : ConcurrentDictionary<Key, Value>
    {
        private readonly Value _defaultValue;

        public ConcurrentDictionaryWithDefault(Value defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public new Value this[Key key]
        {
            get => ContainsKey(key) ? base[key] : _defaultValue;
            set => base[key] = value;
        }
    }
}
