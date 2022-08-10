using System.Collections.ObjectModel;
using SortsVisualizer.lib.Services.Interfaces;

namespace SortsVisualizer.lib.Models.Base;

public abstract class BaseSorting
{
    protected CancellationTokenSource Cts = null!;
    protected IDiagramSourceService DiagramService;
    protected readonly Statistics Statistics;

    public void ResetStatistics()
    {
        Statistics.Reset();
    }

    protected BaseSorting(IDiagramSourceService diagramService, Action<Statistics> statisticUpdater)
    {
        DiagramService = diagramService;
        Statistics = new Statistics(statisticUpdater);
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection, Func<int> getSortSpeed)
    {
        Cts = new CancellationTokenSource();
        try
        {
            await DiagramService.Color.MakeLadderAnimation(CancellationToken.None, System.Windows.Media.Colors.White);
            await SortAsync(collection, Cts.Token, getSortSpeed);
            await DiagramService.Color.MakeLadderAnimation(CancellationToken.None, System.Windows.Media.Colors.Green);
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("Action was interrupted by user.");
        }
        finally
        {
            Cts.Dispose();
            Cts = new CancellationTokenSource();
            await DiagramService.Color.MakeLadderAnimation(CancellationToken.None, System.Windows.Media.Colors.White);
        }
    }

    protected abstract Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel, Func<int> getSortSpeed);

}