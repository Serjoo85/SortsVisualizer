using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services.SubModules;

namespace SortsVisualizer.lib.Services.Interfaces;

public interface IDiagramSourceService
{
    public ObservableCollection<DiagramItem> Items { get; }
    public void Shuffle();

    public Color Color { get; }

    public void CollectionNotify();
}