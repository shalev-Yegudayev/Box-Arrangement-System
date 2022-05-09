using System.Collections.Generic;
using System.Collections.Specialized;

namespace DataStructures
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            if (!TryGetValue(key, out _)) return;
            var index = Keys.Count;
            OnCollectionChanged(NotifyCollectionChangedAction.Add, value, index);
        }

        public new void Remove(TKey key)
        {
            if (!TryGetValue(key, out var value)) return;
            var index = IndexOf(Keys, key);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, value, index);
            base.Remove(key);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private int IndexOf(KeyCollection keys, TKey key)
        {
            var index = 0;
            foreach (var k in keys)
            {
                if (Equals(k, key))
                    return index;
                index++;
            }
            return -1;
        }
    }

}
