using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models.Base;

namespace SortsVisualizer.lib.Models;

public class BubbleOptimizedSorting : BaseSorting, ISorterStrategy
{
    public BubbleOptimizedSorting(IColorChanger colorChanger, Action<Statistics> updateStatistics) : base(colorChanger, updateStatistics)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay)
    {
        _info = new Statistics();
        bool hasSwap1 = false;
        bool hasSwap2 = false;
        int elementsCount = collection.Count;
        do
        {
            hasSwap1 = false;
            for (int i = _info.Iterations; i < elementsCount - _info.Iterations - 1; i++)
            {
                // Красим предыдущий элемент в белый.
                if (i > 0 && collection[i - 1].Color.Color != Colors.Green)
                    ColorChanger.Change(i - 1, Colors.White);
                // Красим текущий в оранжевый.
                ColorChanger.Change(i, Colors.Orange);
                _info.Steps++;
                OnStatisticsChanged(_info);
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
            ColorChanger.Change(elementsCount - _info.Iterations - 1, Colors.Green);
            ColorChanger.ReplacementNotify();

            if (!hasSwap1) return;

            hasSwap2 = false;
            for (int i = elementsCount - _info.Iterations - 1; i > _info.Iterations; i--)
            {
                if (i < elementsCount - 1)
                {
                    if (collection[i + 1].Color.Color != Colors.Green)
                        ColorChanger.Change(i + 1, Colors.White);
                }

                // Красим текущий элемент в оранжевый если он не крайний отсортированный.
                if (i != elementsCount - _info.Iterations - 1)
                {
                    ColorChanger.Change(i, Colors.Orange);
                    _info.Steps++;
                    OnStatisticsChanged(_info);
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
            ColorChanger.Change(_info.Iterations, Colors.Green);
            ColorChanger.ReplacementNotify();

            if (!hasSwap2) return;

            _info.Iterations++;
            OnStatisticsChanged(_info);
        } while (_info.Iterations < elementsCount);
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}