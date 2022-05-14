using System.Windows.Shapes;

namespace SortsVisualizer.lib.Interfaces;

public interface ISorter
{
    Rectangle[] NextStep(Rectangle[] rectangles);
}