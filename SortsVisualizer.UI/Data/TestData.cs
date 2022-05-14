using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SortsVisualizer.UI.Data;

public static class TestData
{
    private static readonly Random _rnd = new Random();
    public static int[] Array => GetMixedArray(20);
    public static ObservableCollection<int> ObservableCollection => GetObservableCollection();
    private static int[] GetMixedArray(int length = 20)
    {
        var array = Enumerable.Range(1, length).Select(n => n * 25).ToArray();
        for (int i = 0; i < length - 4; i++)
        {
            var r = _rnd.Next(1, 4);
            (array[i], array[r]) = (array[r], array[i]);
        }
        return array;
    }

    private static ObservableCollection<int> GetObservableCollection()
    {
        var array = GetMixedArray();
        var collection = new ObservableCollection<int>();
        foreach (var number in array)
        {
            collection.Add(number);
        }
        return collection;
    }
}