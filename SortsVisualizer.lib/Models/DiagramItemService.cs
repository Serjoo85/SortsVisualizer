using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;

namespace SortsVisualizer.lib.Models;

public class DiagramItemService : IDiagramItemService
{
    private ObservableCollection<DiagramItem> _items = null!;
    public ObservableCollection<DiagramItem> Items => _items ??= GetCollection(20);
    public void Shuffle()
    {
        MixCollection(_items);
    }

    private static readonly Random Rnd = new Random();

    private const int HeightFactor = 25;
    private const int Width = 50;

    private static ObservableCollection<DiagramItem> GetCollection(int count)
    {
        var items = new ObservableCollection<DiagramItem>();
        for (int i = 1; i < count + 1; i++)
            items.Add(new DiagramItem
            {
                Value = i,
                Height = i * HeightFactor,
                Width = Width,
                Color = Brushes.White,
            });

        MixCollection(items);

        return items;
    }

    private static void MixCollection(ObservableCollection<DiagramItem> items)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 20 - 3; j++)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j + r]) = (items[j + r], items[j]);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 19; j > 3; j--)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j - r]) = (items[j - r], items[j]);
            }
        }
    }
}