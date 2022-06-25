using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;
using Color = System.Windows.Media;

namespace SortsVisualizer.lib.Services;

public class DiagramSourcesService : IDiagramSourceService
{
    private readonly int _heightFactor;
    private readonly int _width;
    private readonly int _elementsCount;
    private ObservableCollection<DiagramItem> _items = null!;
    public ObservableCollection<DiagramItem> Items => _items ??= GetCollection(_elementsCount);
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;
    private static readonly Random Rnd = new Random();

    public DiagramSourcesService(Action<NotifyCollectionChangedAction> onCollectionChanged, int elementsCount = 20, int heightFactor = 25, int width = 50)
    {
        _heightFactor = heightFactor;
        _width = width;

        _elementsCount = elementsCount;
        _onCollectionChanged = onCollectionChanged;
    }

    private ObservableCollection<DiagramItem> GetCollection(int count)
    {
        var items = new ObservableCollection<DiagramItem>();
        for (int i = 1; i < count + 1; i++)
            items.Add(new DiagramItem
            {
                Value = i,
                Height = i * _heightFactor,
                Width = _width,
                Color = Brushes.White,
            });

        MixCollection(items);
        return items;
    }

    public void Shuffle()
    {
        MixCollection(_items);
    }

    private void MixCollection(ObservableCollection<DiagramItem> items)
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 20 - 3; j++)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j + r]) = (items[j + r], items[j]);
            }

        for (int i = 0; i < 2; i++)
            for (int j = 19; j > 3; j--)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j - r]) = (items[j - r], items[j]);
            }
    }

    #region IColorChanger
    public void Change(
        int index,
        System.Windows.Media.Color color)
    {
        var newItem = Items[index];
        newItem.Color = new SolidColorBrush(color);
        Items[index] = newItem;
    }

    Task IColorChanger.FillAllWithAnimation(CancellationToken cancel, System.Windows.Media.Color color, int delay)
    {
        return FillAllWithAnimation(cancel, color, delay);
    }

    public async Task FillAllWithAnimation(
        CancellationToken cancel,
        System.Windows.Media.Color color,
        int delay = 50)
    {
        var x = Thread.CurrentThread.ManagedThreadId;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Color.Color == color)
                continue;
            Change(i, color);
            await Task.Delay(delay, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }
    #endregion
    

    public void ReplacementNotify()
    {
        _onCollectionChanged.Invoke(NotifyCollectionChangedAction.Replace);
    }
}