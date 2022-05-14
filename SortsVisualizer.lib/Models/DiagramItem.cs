using System.Windows.Media;

namespace SortsVisualizer.lib.Models;

public struct DiagramItem
{
    public int Value { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public SolidColorBrush Color { get; set; }
}