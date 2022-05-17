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
        CancellationToken cancel, Action<int> action, int delay = 100)
    {
        Action = action;
        StepCount = 0;
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
                        ColorChanger.Change(j - 1, Colors.White);
                    ColorChanger.Change(j, Colors.Orange);
                    ColorChanger.ReplacementNotify();
                    StepCount++;
                    await Task.Delay(delay, cancel);
                    (collection[j], collection[j + 1]) = (collection[j + 1], collection[j]);
                    ColorChanger.ReplacementNotify();
                    // При перестановки получается перемещение, должна быть задержка.
                    await Task.Delay(delay, cancel);

                }
                // Закрашиваем текущий элемент без перестановки.
                else
                {

                    if (j > 0)
                        ColorChanger.Change(j - 1, Colors.White);
                    ColorChanger.Change(j, Colors.Orange);
                    StepCount++;
                    ColorChanger.ReplacementNotify(); ;
                    await Task.Delay(delay, cancel);
                }
            }

            // Красим в белый последний закрашенный прямоугольник.
            ColorChanger.Change(num - i - 1, Colors.Green);
            ColorChanger.Change(num - i - 2, Colors.White);

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