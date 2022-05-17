using System.Collections.ObjectModel;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services;

public class SorterService : ISorterService
{
    private readonly ObservableCollection<DiagramItem> _collection;
    private readonly Dictionary<SortType, ISorterStrategy> _sorters;
    private ISorterStrategy _startedStrategy = null!;
    private TimeSpan _time;

    public SorterService(IColorChanger colorChanger, ObservableCollection<DiagramItem> collection)
    {
        _collection = collection;
        _sorters = new()
        {
            { SortType.Bubble, new BubbleSorting(colorChanger) },
            { SortType.OptimizedBubble, new BubbleOptimizedSorting(colorChanger) },
        };
    }

    /// <summary>
    /// Возвращает сортировку заданного типа
    /// </summary>
    /// <param name="type">Тип сортировки</param>
    /// <param name="action">Обратный вызов</param>
    /// <returns></returns>
    public async Task StartAsync(SortType type, Action<int> action)
    {
        if (!_sorters.ContainsKey(type))
            throw new ArgumentException(nameof(type));
        _startedStrategy = _sorters[type];
        await _startedStrategy.StartAsync(_collection, action);
    }

    public void Stop()
    {
        _startedStrategy?.Stop();
        _startedStrategy = null!;
    }

    public string[] GetSortersTypes()
    {
        return _sorters.Select(s => s.Key.ToString()).ToArray();
    }
}