using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Models.Base;
using SortsVisualizer.lib.Models.Interfaces;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Models.Sorts;

public class QuickSort : BaseSorting, ISorterStrategy
{
    public QuickSort(IDiagramSourceService diagramService, Action<Statistics> statisticUpdater) : base(diagramService, statisticUpdater)
    {
    }

    protected override async Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel, int delay)
    {
        await SortArray(collection, 0, 19, cancel, delay);
    }

    public async Task SortArray(ObservableCollection<DiagramItem> collection, int leftIndex, int rightIndex, CancellationToken cancel, int delay)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = collection[leftIndex].Value;
        while (i <= j)
        {
            while (collection[i].Value < pivot)
            {
                i++;
            }

            while (collection[j].Value > pivot)
            {
                j--;
            }
            if (i <= j)
            {
                await Task.Delay(delay, cancel);
                (collection[i], collection[j]) = (collection[j], collection[i]);
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            await SortArray(collection, leftIndex, j, cancel, delay);
        if (i < rightIndex)
            await SortArray(collection, i, rightIndex, cancel, delay);
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}