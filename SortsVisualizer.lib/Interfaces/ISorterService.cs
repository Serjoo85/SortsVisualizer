using System.Collections.ObjectModel;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Interfaces;

public interface ISorterService
{
    public Task StartAsync(SortType type, Action<int> action);
    public void Stop();
    public string[] GetSortersTypes();
}