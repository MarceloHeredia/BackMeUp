using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BackMeUp.Utils.ExtensionMethods
{
    internal static class ObservableCollectionExtensions 
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (condition(collection[i]))
                {
                    collection.RemoveAt(i);
                }
            }
        }
    }
}
