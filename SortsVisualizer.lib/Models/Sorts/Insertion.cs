using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using SortsVisualizer.lib.Models.Base;
using SortsVisualizer.lib.Models.Interfaces;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Models.Sorts
{
    public class Insertion : BaseSorting, ISorterStrategy
    {
        public Insertion(IDiagramSourceService diagramService, Action<Statistics> updateStatistics) : base(diagramService, updateStatistics)
        {
        }

        protected override async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel,
        int delay)
        {
            int j;
            int x;
            DiagramService.Color.Change(0, Colors.Green);
            for (int i = 1; i < collection.Count; i++)
            {
                x = collection[i].Value;
                j = i;
                while (j > 0 && collection[j - 1].Value > x)
                {
                    DiagramService.Color.Change(j, Colors.Orange);
                    DiagramService.CollectionNotify();
                    await Task.Delay(500, cancel);
                    (collection[j], collection[j - 1]) = (collection[j - 1], collection[j]);
                    await Task.Delay(500, cancel);
                    DiagramService.CollectionNotify();
                    Info.Steps++;
                    j -= 1;
                }
                DiagramService.Color.Change(j, Colors.Green);
            }
        }

        public void Stop()
        {
            Cts.Cancel();
        }
    }
}
