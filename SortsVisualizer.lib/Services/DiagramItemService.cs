using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services;

public class DiagramItemService : IDiagramItemService
{
    private ObservableCollection<DiagramItem> _items = null!; public ObservableCollection<DiagramItem> Items => _items ??= GetCollection(20);
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;


    private static readonly Random Rnd = new Random();

    private const int HeightFactor = 25;
    private const int Width = 50;

    public DiagramItemService(Action<NotifyCollectionChangedAction> onCollectionChanged)
    {
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

    public void Change(int index, Color color, ObservableCollection<DiagramItem> collection)
    {
        var newItem = collection[index];
        newItem.Color = new SolidColorBrush(color);
        collection[index] = newItem;
    }

    public async Task FinishPaint(ObservableCollection<DiagramItem> collection, CancellationToken cancel)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            if (collection[i].Color.Color == Colors.Green)
                return;
            Change(i, Colors.Green, collection);
            await Task.Delay(50, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }


    public async Task FillAll(ObservableCollection<DiagramItem> collection, CancellationToken cancel)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            if(collection[i].Color.Color == Colors.White)
                continue;
            Change(i, Colors.White, collection);
            await Task.Delay(50, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }

    public void ReplacementNotify()
    {
        _onCollectionChanged.Invoke(NotifyCollectionChangedAction.Replace);
    }
}