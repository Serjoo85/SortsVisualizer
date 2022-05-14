using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Interfaces;

public interface ISorterStrategy
{
    Task StartAsync(ObservableCollection<DiagramItem> collection);
    void Stop();
}