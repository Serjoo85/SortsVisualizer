using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models.Base;

namespace SortsVisualizer.lib.Models;

public class BubbleSorting : BaseSorting, ISorterStrategy
{
    public BubbleSorting(IColorChanger colorChanger) : base(colorChanger)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay = 100)
    {
        int num = collection.Count;
        for (int i = 0; i < num - 1; i++)
        {
            var hasSwap = false;
            for (int j = 0; j < num - i - 1; j++)
            {
                // j - индекс текущего элемента.

                // Закрашиваем текущий элемент с перестановкой.
                if (collection[j].Value > collection[j + 1].Value)
                {
                    hasSwap = true;

                    if (j > 0)
                        ColorChanger.Change(j - 1, Colors.White, collection);
                    ColorChanger.Change(j, Colors.Orange, collection);
                    ColorChanger.ReplacementNotify(); ;
                    await Task.Delay(delay - 10, cancel);
                    (collection[j], collection[j + 1]) = (collection[j + 1], collection[j]);
                    ColorChanger.ReplacementNotify(); ;
                    await Task.Delay(delay - 10, cancel);

                }
                // Закрашиваем текущий элемент без перестановки.
                else
                {

                    if (j > 0)
                        ColorChanger.Change(j - 1, Colors.White, collection);
                    ColorChanger.Change(j, Colors.Orange, collection);
                    ColorChanger.ReplacementNotify(); ;
                    await Task.Delay(delay, cancel);
                }
            }

            // Красим в белый последний закрашенный прямоугольник.
            ColorChanger.Change(num - i - 1, Colors.Green, collection);
            ColorChanger.Change(num - i - 2, Colors.White, collection);

            ColorChanger.ReplacementNotify(); ;
            // Проход без замены признак отсортированной последовательности.
            if (hasSwap == false)
            {
                return;
            }
        }
    }

    public void Stop()
    {
        Cts.Cancel();
    }
}