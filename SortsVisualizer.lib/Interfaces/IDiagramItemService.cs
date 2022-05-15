using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Interfaces;

public interface IDiagramItemService
{
    public ObservableCollection<DiagramItem> Items { get; }
    public  void Shuffle();
}