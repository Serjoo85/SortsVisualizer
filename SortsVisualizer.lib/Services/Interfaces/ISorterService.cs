using System.Collections.ObjectModel;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services.Interfaces;

public interface ISorterService
{
    public Task StartAsync(SortType type, ObservableCollection<DiagramItem> collection, int delay);
    public void Stop();
    public string[] GetSortersTypes();
}