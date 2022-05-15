using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;

namespace SortsVisualizer.lib.Models.Base;

public abstract class BaseSorting
{
    protected CancellationTokenSource _cts = null!;
    protected readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;

    protected BaseSorting(Action<NotifyCollectionChangedAction> onCollectionChanged)
    {
        _onCollectionChanged = onCollectionChanged;
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection)
    {
        _cts = new CancellationTokenSource();
        try
        {
            await SortAsync(collection, _cts.Token);
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("Action was interrupted by user.");
        }
        finally
        {
            _cts.Dispose();
        }
    }

    protected abstract Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel);

    protected void ChangeColor(int index, Color color, ObservableCollection<DiagramItem> collection)
    {
        var newItem = collection[index];
        newItem.Color = new SolidColorBrush(color);
        collection[index] = newItem;
    }

    protected async Task FinishPaint(ObservableCollection<DiagramItem> collection, CancellationToken cancel)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            if (collection[i].Color.Color == Colors.Green)
                return;
            ChangeColor(i, Colors.Green, collection);
            await Task.Delay(50, cancel);
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
        }
    }
}