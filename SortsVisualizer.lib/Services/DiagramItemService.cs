using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services;

public class DiagramItemService : IDiagramItemService
{
    private const int HeightFactor = 25;
    private const int Width = 50;
    private readonly int _elementCount;
    private ObservableCollection<DiagramItem> _items = null!; 
    public ObservableCollection<DiagramItem> Items => _items ??= GetCollection(_elementCount);
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;
    private static readonly Random Rnd = new Random();

    public DiagramItemService(Action<NotifyCollectionChangedAction> onCollectionChanged, int elementCount = 20)
    {
        _elementCount = elementCount;
        _onCollectionChanged = onCollectionChanged;
    }

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

    public void Shuffle()
    {
        MixCollection(_items);
    }

    private static void MixCollection(ObservableCollection<DiagramItem> items)
    {
        for (int i = 0; i < 2; i++)
        for (int j = 0; j < 20 - 3; j++)
        {
            var r = Rnd.Next(1, 3);
            (items[j], items[j + r]) = (items[j + r], items[j]);
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

    public void Change(int index, Color color, ObservableCollection<DiagramItem> collection)
    {
        var newItem = collection[index];
        newItem.Color = new SolidColorBrush(color);
        collection[index] = newItem;
    }

    public async Task FillAllWithAnimation(ObservableCollection<DiagramItem> collection, CancellationToken cancel, Color color, int delay = 50)
    {
        var x = Thread.CurrentThread.ManagedThreadId;

        for (int i = 0; i < collection.Count; i++)
        {
            if(collection[i].Color.Color == color)
                continue;
            Change(i, color, collection);
            await Task.Delay(delay, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }

    public void ReplacementNotify()
    {
        _onCollectionChanged.Invoke(NotifyCollectionChangedAction.Replace);
    }
}