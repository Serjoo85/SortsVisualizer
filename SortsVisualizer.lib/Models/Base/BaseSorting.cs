using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;

namespace SortsVisualizer.lib.Models.Base;

public abstract class BaseSorting
{
    protected CancellationTokenSource Cts = null!;
    protected IColorChanger ColorChanger;

    protected BaseSorting(IColorChanger colorChanger)
    {
        ColorChanger = colorChanger;
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection)
    {
        Cts = new CancellationTokenSource();
        try
        {
            await ColorChanger.FillAllWithAnimation(collection, CancellationToken.None, Colors.White);
            await SortAsync(collection, Cts.Token, 80);
            await ColorChanger.FillAllWithAnimation(collection, CancellationToken.None, Colors.Green);
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("Action was interrupted by user.");
        }
        finally
        {
            Cts.Dispose();
            Cts = new CancellationTokenSource();
            await ColorChanger.FillAllWithAnimation(collection, CancellationToken.None, Colors.White);
        }
    }

    protected abstract Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel, int delay = 100);

}