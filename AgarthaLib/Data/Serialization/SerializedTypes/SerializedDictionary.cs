using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgarthaLib.Data.Serialization.SerializedTypes
{
    [Serializable] public class SerializedDictionary<K, V> : IDictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializedKeyValuePair<K, V>> _data = new();
        [NonSerialized] private Dictionary<K, V> _dictionary;

        public List<SerializedKeyValuePair<K, V>> Data
        {
            get => _data;
            set => _data = value ?? new();
        }

        public Dictionary<K, V> Dictionary => EnsureNotNull();
        private Dictionary<K, V> EnsureNotNull()
        {
            if (_dictionary == null) _dictionary = new();
            return _dictionary;
        }

        public SerializedDictionary()
            => _dictionary = new();

        public SerializedDictionary(int capacity, IEqualityComparer<K> comparer = null)
            => _dictionary = new(capacity, comparer ?? EqualityComparer<K>.Default);

        #region ISerializationCallbackReciever

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            Dictionary.Clear();

            if (Data == null || Dictionary == null) return;
            foreach (var kvp in Data)
            {
                if (kvp.Key == null) continue;
                Dictionary[kvp.Key] = kvp.Value;
            }
        }

        #endregion

        #region IDictionary

        public ICollection<K> Keys => Dictionary.Keys;

        public ICollection<V> Values => Dictionary.Values;

        public int Count => Dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(K key, V value)
        {
           
            Dictionary.Add(key, value);
            Data.Add(new SerializedKeyValuePair<K, V>(key, value));
        }

        public bool ContainsKey(K key)
            => Dictionary.ContainsKey(key);

        public bool Remove(K key)
        {
            if (!Dictionary.Remove(key))
                return false;

            Data.RemoveAll(kvp => EqualityComparer<K>.Default.Equals(kvp.Key, key));
            return true;
        }

        public bool TryGetValue(K key, out V value)
            => Dictionary.TryGetValue(key, out value);

        public void Add(KeyValuePair<K, V> item)
            => Add(item.Key, item.Value);

        public void Clear()
        {
            Dictionary.Clear();
            Data.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
            => Dictionary.TryGetValue(item.Key, out var v) && EqualityComparer<V>.Default.Equals(v, item.Value);

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<K, V>>)Dictionary).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<K, V> item)
            => Contains(item) && Dictionary.Remove(item.Key);

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
            => Dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public V this[K key]
        {
            get => Dictionary[key];
            set
            {
                Dictionary[key] = value;
                var idx = Data.FindIndex(kvp => EqualityComparer<K>.Default.Equals(kvp.Key, key));

                if (idx >= 0) Data[idx] = new SerializedKeyValuePair<K, V>(key, value);
                else Data.Add(new SerializedKeyValuePair<K, V>(key, value));
            }
        }

        #endregion

        [ContextMenu("Fill with blanks")]
        public void FillWithBlanks()
        {
            Dictionary.Clear();

            if (typeof(K).IsEnum)
            {
                var values = Enum.GetValues(typeof(K));
                foreach (var v in values)
                    Dictionary.Add((K)v, default);
            }
        }
    }
}