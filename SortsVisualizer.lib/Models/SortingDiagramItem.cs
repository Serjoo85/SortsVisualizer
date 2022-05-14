using System.Windows.Media;

namespace SortsVisualizer.lib.Models;

public struct SortingDiagramItem
{
    public int Value { get; }
    public int Height { get; }
    public int Width { get; }
    public Brushes Colour { get; }
}