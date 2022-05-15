using System.Collections.Specialized;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services;

public class SorterService : ISorterService
{
    private readonly Dictionary<SortType, ISorterStrategy> _sorters;

    public SorterService(Action<NotifyCollectionChangedAction> onCollectionChanged)
    {
        _sorters = new()
        {
            { SortType.Bubble, new BubbleSorting(onCollectionChanged) },

        };
    }

    /// <summary>
    /// Возвращает сортировку заданного типа
    /// </summary>
    /// <param name="type">Тип сортировки</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public ISorterStrategy GetSorter(SortType type)
    {
        if (!_sorters.ContainsKey(type))
            throw new ArgumentException(nameof(type));
        return _sorters[type];
    }

    public string[] GetSortersTypes()
    {
        return _sorters.Select(s => s.Key.ToString()).ToArray();
    }
}