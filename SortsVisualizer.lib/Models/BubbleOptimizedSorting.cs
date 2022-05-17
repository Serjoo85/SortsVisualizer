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
        CancellationToken cancel, Action<int> action, int delay = 100)
    {
        Action = action;
        StepCount = 0;
        int nonSortedElement = collection.Count;
        bool hasSwap1 = false;
        bool hasSwap2 = false;
        int elementsCount = collection.Count;
        int interactionsCount = 0;
        do
        {
            hasSwap1 = false;
            for (int i = interactionsCount; i < elementsCount - interactionsCount - 1; i++)
            {
                // Красим предыдущий элемент в белый.
                if (i > 0 && collection[i - 1].Color.Color != Colors.Green)
                    ColorChanger.Change(i - 1, Colors.White);
                // Красим текущий в оранжевый.
                ColorChanger.Change(i, Colors.Orange);
                StepCount++;
                ColorChanger.ReplacementNotify();
                

                await Task.Delay(delay, cancel);

                // Меняем местами если больше.
                if (collection[i].Value > collection[i + 1].Value)
                {
                    (collection[i], collection[i + 1]) = (collection[i + 1], collection[i]);
                    ColorChanger.ReplacementNotify();
                    await Task.Delay(delay, cancel);
                    hasSwap1 = true;
                }
            }

            // Красим зелёным последний отсортированный элемент.
            ColorChanger.Change(elementsCount - interactionsCount - 1, Colors.Green);
            ColorChanger.ReplacementNotify();

            if (!hasSwap1) return;
            nonSortedElement--;
            hasSwap2 = false;
            for (int i = elementsCount - interactionsCount - 1; i > interactionsCount; i--)
            {
                if (i < elementsCount - 1)
                {
                    if (collection[i + 1].Color.Color != Colors.Green)
                        ColorChanger.Change(i + 1, Colors.White);
                }

                // Красим текущий элемент в оранжевый если он не крайний отсортированный.
                if (i != elementsCount - interactionsCount - 1)
                {
                    ColorChanger.Change(i, Colors.Orange);
                    StepCount++;
                }

                ColorChanger.ReplacementNotify();

                await Task.Delay(delay, cancel);

                // Меняем местами если меньше.
                if (collection[i].Value < collection[i - 1].Value)
                {
                    (collection[i], collection[i - 1]) = (collection[i - 1], collection[i]);
                    ColorChanger.ReplacementNotify();
                    await Task.Delay(delay, cancel);
                    hasSwap2 = true;
                }
            }

            // Красим зелёным последний отсортированный элемент.
            ColorChanger.Change(interactionsCount, Colors.Green);
            ColorChanger.ReplacementNotify();

            if (!hasSwap2) return;
            nonSortedElement--;

            interactionsCount++;
        } while (nonSortedElement > 0);
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}