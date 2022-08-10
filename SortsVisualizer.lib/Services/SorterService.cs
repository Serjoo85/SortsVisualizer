using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Models.Interfaces;
using SortsVisualizer.lib.Models.Sorts;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Services;

public class SorterService : ISorterService
{
    private readonly Dictionary<SortType, ISorterStrategy> _sorters;
    private ISorterStrategy _startedStrategy = null!;

    public SorterService(IDiagramSourceService diagramService, Action<Statistics> updateStatistics)
    {
        _sorters = new()
        {
            { SortType.Bubble, new Bubble(diagramService, updateStatistics)},
            { SortType.Shaker, new Shaker(diagramService, updateStatistics) },
            { SortType.Insertion, new Insertion(diagramService, updateStatistics) },
            { SortType.QuickSort, new QuickSort(diagramService, updateStatistics) },
        };
    }

    /// <summary>
    /// Возвращает сортировку заданного типа
    /// </summary>
    /// <param name="type">Тип сортировки</param>
    /// <param name="action">Обратный вызов</param>
    /// <param name="collection"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public async Task StartAsync(SortType type, ObservableCollection<DiagramItem> collection, int delay = 80)
    {
        if (!_sorters.ContainsKey(type))
            throw new ArgumentException(nameof(type));
        _startedStrategy = _sorters[type];
        await _startedStrategy.StartAsync(collection, delay);
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