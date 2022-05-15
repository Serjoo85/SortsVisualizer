using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services;

public class SorterService : ISorterService
{
    private readonly Dictionary<SortType, ISorterStrategy> _sorters;
    private ISorterStrategy _startedStrategy = null!;

    public SorterService(IColorChanger colorChanger)
    {
        _sorters = new()
        {
            { SortType.Bubble, new BubbleSorting(colorChanger) },
        };
    }

    /// <summary>
    /// Возвращает сортировку заданного типа
    /// </summary>
    /// <param name="type">Тип сортировки</param>
    /// <param name="collection">Сортируемая коллекция</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task StartAsync(SortType type, ObservableCollection<DiagramItem> collection)
    {
        if (!_sorters.ContainsKey(type))
            throw new ArgumentException(nameof(type));
        _startedStrategy = _sorters[type];
        await _startedStrategy.StartAsync(collection);
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