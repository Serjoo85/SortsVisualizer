using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Models.Base;
using SortsVisualizer.lib.Models.Interfaces;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Models.Sorts;

public class Bubble : BaseSorting, ISorterStrategy
{
    public Bubble(IDiagramSourceService diagramService, Action<Statistics> updateStatistics) : base(diagramService, updateStatistics)
    {
    }

    protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel, int delay)
    {
        int num = collection.Count;
        for (int i = 0; i < num - 1; i++)
        {
            var hasSwap = false;
            
            for (int j = 0; j < num - i - 1; j++)
            {
                // j - индекс текущего элемента.
                Info.Comparison++;
                // Закрашиваем текущий элемент с перестановкой.
                if (collection[j].Value > collection[j + 1].Value)
                {
                    hasSwap = true;

                    if (j > 0)
                        DiagramService.Color.Change(j - 1, Colors.White);
                    DiagramService.Color.Change(j, Colors.Orange);
                    DiagramService.CollectionNotify();
                    await Task.Delay(delay, cancel);
                    (collection[j], collection[j + 1]) = (collection[j + 1], collection[j]);
                    Info.Replacement++;
                    DiagramService.CollectionNotify();
                    // При перестановке получается перемещение, должна быть задержка.
                    await Task.Delay(delay, cancel);

                }
                // Закрашиваем текущий элемент без перестановки.
                else
                {
                    if (j > 0)
                        DiagramService.Color.Change(j - 1, Colors.White);
                    DiagramService.Color.Change(j, Colors.Orange);
                    DiagramService.CollectionNotify();
                    await Task.Delay(delay, cancel);
                }
            }

            // Отмечаем зелёным последний элемент как отсортированный.
            DiagramService.Color.Change(num - i - 1, Colors.Green);
            /*  Красим в белый предпоследний прямоугольник иначе
                на последней итерации будет оранжевая полоса.
                TODO Нужно оптимизировать.
            */
            DiagramService.Color.Change(num - i - 2, Colors.White);
            DiagramService.CollectionNotify(); ;

            Info.Comparison++;
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
        Info.Reset();
    }
}