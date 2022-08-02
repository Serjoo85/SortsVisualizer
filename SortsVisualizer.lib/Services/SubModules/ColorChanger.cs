using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Services.SubModules;

public class ColorChanger : IColorChanger
{
    private readonly ObservableCollection<DiagramItem> _items = null!;
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;

    public ColorChanger(ObservableCollection<DiagramItem> items, Action<NotifyCollectionChangedAction> onCollectionChanged)
    {
        _items = items;
        _onCollectionChanged = onCollectionChanged;
    }

    public void FillSingleRectangle(
        int index,
        System.Windows.Media.Color color)
    {
        var newItem = _items[index];
        newItem.Color = new SolidColorBrush(color);
        _items[index] = newItem;
    }

    public void Change(
        int index,
        System.Windows.Media.Color color)
    {
        var newItem = _items[index];
        newItem.Color = new SolidColorBrush(color);
        _items[index] = newItem;
    }

    public async Task MakeLadderAnimation(
        CancellationToken cancel,
        System.Windows.Media.Color color,
        int delay = 80)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Color.Color == color)
                continue;
            Change(i, color);
            await Task.Delay(delay, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }
}