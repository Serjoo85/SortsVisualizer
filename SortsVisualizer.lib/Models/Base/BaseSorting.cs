using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;

namespace SortsVisualizer.lib.Models.Base;

public abstract class BaseSorting
{
    protected CancellationTokenSource Cts = null!;
    protected IColorChanger ColorChanger;

    public event Action<object> StatisticsChanged;

    protected Action<int> Action;
    protected int _stepCount;

    protected int StepCount
    {
        get => _stepCount;
        set
        {
            _stepCount = value;
            Action?.Invoke(_stepCount);
        }
    }

    protected BaseSorting(IColorChanger colorChanger)
    {
        ColorChanger = colorChanger;
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection, Action<int> action)
    {
        Cts = new CancellationTokenSource();
        try
        {
            await ColorChanger.FillAllWithAnimation(CancellationToken.None, Colors.White);
            await SortAsync(collection, Cts.Token, action, 80);
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

    protected abstract Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel, Action<int> action, int delay = 100);

}