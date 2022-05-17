using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models.Base;

namespace SortsVisualizer.lib.Models;

public class BubbleSorting : BaseSorting, ISorterStrategy
{
    public BubbleSorting(IColorChanger colorChanger, Action<Statistics> updateStatistics) : base(colorChanger, updateStatistics)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay)
    {
        _info = new Statistics();

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

                    _info.Steps++;
                    OnStatisticsChanged(_info);

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
                    ColorChanger.ReplacementNotify(); ;

                    _info.Steps++;
                    OnStatisticsChanged(_info);
                    await Task.Delay(delay, cancel);
                }
            }

            // Отмечаем зелёным последний элемент как отсортированный.
            ColorChanger.Change(num - i - 1, Colors.Green);
            /*  Красим в белый предпоследний прямоугольник иначе
                на последней итерации будет оранжевая полоса.
                TODO Нужно оптимизировать.
            */
            ColorChanger.Change(num - i - 2, Colors.White);
            ColorChanger.ReplacementNotify(); ;

            _info.Iterations++;
            OnStatisticsChanged(_info);
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