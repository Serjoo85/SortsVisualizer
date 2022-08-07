namespace SortsVisualizer.lib.Models;

public class Statistics
{
    private readonly Action<Statistics> _statisticUpdater;
    private int _steps;
    private int _iterations;

    public Statistics(Action<Statistics> statisticUpdater)
    {
        _statisticUpdater = statisticUpdater;
    }

    public int Steps
    {
        get => _steps;
        set
        {
            _steps = value;
            Update();
        }
    }

    public int Iterations
    {
        get => _iterations;
        set
        {
            _iterations = value;
            Update();
        }
    }

    public void Reset()
    {
        Iterations = 0;
        Steps = 0;
    }

    private void Update()
    {
        _statisticUpdater.Invoke(this);
    }
}