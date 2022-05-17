using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;

namespace SortsVisualizer.lib.Models.Base;

public abstract class BaseSorting
{
    protected CancellationTokenSource Cts = null!;
    protected IColorChanger ColorChanger;
    protected event Action<Statistics> StatisticsChanged;
    protected Statistics _info = null!;

    protected BaseSorting(IColorChanger colorChanger, Action<Statistics> updateStatistics)
    {
        StatisticsChanged += updateStatistics;
        ColorChanger = colorChanger;
    }

    protected void OnStatisticsChanged(Statistics info)
    {
        StatisticsChanged.Invoke(info);
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection,int delay)
    {
        Cts = new CancellationTokenSource();
        try
        {
            await ColorChanger.FillAllWithAnimation(CancellationToken.None, Colors.White);
            await SortAsync(collection, Cts.Token,delay);
            await ColorChanger.FillAllWithAnimation(CancellationToken.None, Colors.Green);
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("Action was interrupted by user.");
        }
        finally
        {
            Cts.Dispose();
            Cts = new CancellationTokenSource();
            await ColorChanger.FillAllWithAnimation(CancellationToken.None, Colors.White);
        }
    }

    protected abstract Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel, int delay);

}