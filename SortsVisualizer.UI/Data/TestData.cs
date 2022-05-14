using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.UI.Data;

public static class TestData
{
    private static readonly Random Rnd = new Random();
    public static ObservableCollection<DiagramItem> ObservableCollection => GetCollection(20);

    private const int HeightFactor = 25;
    private const int Width = 50;

    private static ObservableCollection<DiagramItem> GetCollection(int count)
    {
        var collection = new ObservableCollection<DiagramItem>();
        for (int i = 1; i < count + 1; i++)
            collection.Add(new DiagramItem
            {
                Value = i,
                Height = i * HeightFactor,
                Width = Width,
                Color = Brushes.White,
            });

        MixCollection(collection);

        return collection;
    }

    private static void MixCollection(ObservableCollection<DiagramItem> collection)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 20 - 3; j++)
            {
                var r = Rnd.Next(1, 3);
                (collection[j], collection[r]) = (collection[r], collection[j]);
            }
        }
    }
}