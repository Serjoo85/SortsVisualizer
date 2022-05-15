using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models.Base;

namespace SortsVisualizer.lib.Models;

public class BubbleOptimizedSorting : BaseSorting, ISorterStrategy
{
    public BubbleOptimizedSorting(IColorChanger colorChanger) : base(colorChanger)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay = 100)
    {
        int nonSortedElement = collection.Count;
        bool hasSwap1 = false;
        bool hasSwap2 = false;

        do
        {
            hasSwap1 = false;
            for (int i = 0; i < nonSortedElement - 1; i++)
            {
                
                if (collection[i].Value > collection[i + 1].Value)
                {
                    (collection[i], collection[i + 1]) = (collection[i + 1], collection[i]);
                    ColorChanger.ReplacementNotify();
                    hasSwap1 = true;
                }

                await Task.Delay(delay, cancel);
            }

            if(!hasSwap1 && !hasSwap2) return;
            nonSortedElement--;
            hasSwap2 = false;
            for (int i = nonSortedElement - 1; i > 0; i--)
            {
                if (collection[i].Value < collection[i - 1].Value)
                {
                    (collection[i], collection[i - 1]) = (collection[i - 1], collection[i]);
                    ColorChanger.ReplacementNotify();
                    hasSwap2 = true;
                }

                await Task.Delay(delay, cancel);
            }
            if (!hasSwap1 && !hasSwap2) return;
            nonSortedElement--;

        } while (nonSortedElement > 0);
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}