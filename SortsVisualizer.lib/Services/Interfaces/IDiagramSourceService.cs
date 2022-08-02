using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services.Interfaces;

public interface IDiagramSourceService : IColorChanger
{
    public ObservableCollection<DiagramItem> Items { get; }
    public void Shuffle();
}