using SortsVisualizer.lib.Enums;

namespace SortsVisualizer.lib.Interfaces;

public interface ISorterService
{
    public ISorterStrategy GetSorter(SortType type);
    public string[] GetSortersTypes();
}