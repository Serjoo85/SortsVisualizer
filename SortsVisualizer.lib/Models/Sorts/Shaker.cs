using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Drawing;
using SortsVisualizer.lib.Models.Base;
using SortsVisualizer.lib.Models.Interfaces;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Models.Sorts;

public class Shaker : BaseSorting, ISorterStrategy
{
    public Shaker(IDiagramSourceService colorChanger, Action<Statistics> statisticUpdater) : base(colorChanger, statisticUpdater)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay)
    {
        bool hasSwap1 = false;
        bool hasSwap2 = false;
        int elementsCount = collection.Count;
        do
        {
            hasSwap1 = false;
            for (int i = Info.Iterations; i < elementsCount - Info.Iterations - 1; i++)
            {
                // Красим предыдущий элемент в белый.
                if (i > 0 && collection[i - 1].Color.Color != Colors.Green)
                    DiagramService.Color.Change(i - 1, Colors.White);
                // Красим текущий в оранжевый.
                DiagramService.Color.Change(i, Colors.Orange);
                Info.Steps++;
                DiagramService.CollectionNotify();


                await Task.Delay(delay, cancel);

                // Меняем местами если больше.
                if (collection[i].Value > collection[i + 1].Value)
                {
                    (collection[i], collection[i + 1]) = (collection[i + 1], collection[i]);
                    DiagramService.CollectionNotify();
                    await Task.Delay(delay, cancel);
                    hasSwap1 = true;
                }
            }

            // Красим зелёным последний отсортированный элемент.
            DiagramService.Color.Change(elementsCount - Info.Iterations - 1, Colors.Green);
            DiagramService.CollectionNotify();

            if (!hasSwap1) return;

            hasSwap2 = false;
            for (int i = elementsCount - Info.Iterations - 1; i > Info.Iterations; i--)
            {
                if (i < elementsCount - 1)
                {
                    if (collection[i + 1].Color.Color != Colors.Green)
                        DiagramService.Color.Change(i + 1, Colors.White);
                }

                // Красим текущий элемент в оранжевый если он не крайний отсортированный.
                if (i != elementsCount - Info.Iterations - 1)
                {
                    DiagramService.Color.Change(i, Colors.Orange);
                    Info.Steps++;
                }

                DiagramService.CollectionNotify();

                await Task.Delay(delay, cancel);

                // Меняем местами если меньше.
                if (collection[i].Value < collection[i - 1].Value)
                {
                    (collection[i], collection[i - 1]) = (collection[i - 1], collection[i]);
                    DiagramService.CollectionNotify();
                    await Task.Delay(delay, cancel);
                    hasSwap2 = true;
                }
            }

            // Красим зелёным последний отсортированный элемент.
            DiagramService.Color.Change(Info.Iterations, Colors.Green);
            DiagramService.CollectionNotify();

            if (!hasSwap2) return;

            Info.Iterations++;
        } while (Info.Iterations < elementsCount);
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}