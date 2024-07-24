using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace DispoDataAssistant.Extensions;

public static class ObservableCollectionExtensions
{
    public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
    {
        var sorted = collection.OrderBy(x => x, Comparer<T>.Create(comparison)).ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            collection.Move(collection.IndexOf(sorted[i]), i);
        }
    }

    public static void Sort<T, TKey>(this ObservableCollection<T> collection, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        var sorted = collection.OrderBy(keySelector).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            // Find the index of the item that should be at the i-th position in the sorted collection
            int originalIndex = collection.IndexOf(sorted[i]);
            if (i != originalIndex)
            {
                // Move the item to its new position if it isn't already there
                collection.Move(originalIndex, i);
            }
        }
    }


}
