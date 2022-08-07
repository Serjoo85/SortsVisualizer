namespace SortsVisualizer.lib.Models;

public class Statistics
{
    private readonly Action<Statistics> _statisticUpdater;
    private int _replacement;
    private int _comparison;

    public Statistics(Action<Statistics> statisticUpdater)
    {
        _statisticUpdater = statisticUpdater;
    }

    public int Replacement
    {
        get => _replacement;
        set
        {
            _replacement = value;
            Update();
        }
    }

    public int Comparison
    {
        get => _comparison;
        set
        {
            _comparison = value;
            Update();
        }
    }

    public void Reset()
    {
        Comparison = 0;
        Replacement = 0;
    }

    private void Update()
    {
        _statisticUpdater.Invoke(this);
    }
}