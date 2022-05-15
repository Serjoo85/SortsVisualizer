using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Interfaces;

public interface IDiagramItemService : IColorChanger
{
    public ObservableCollection<DiagramItem> Items { get; }
    public  void Shuffle();
}