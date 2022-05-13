using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MailSender.lib.Commands;
using SortsVisualizer.lib.Services;
using SortsVisualizer.UI.Data;

namespace SortsVisualizer.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Base functionality INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion

    public MainWindowViewModel()
    {
        DiagramSource = TestData.array;
        _bubbleSort = new BubbleSorting();

        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
    }

    #region Commands

    public ICommand StartSortingCommand { get; }

    private bool CanStartSortingCommandExecute(object o) => true;

    private async void OnStartSortingCommandExecuted(object o)
    {
        await BubbleSort();
    }

    #endregion

    private int[] _diagramSource;
    private readonly BubbleSorting _bubbleSort;
    public int[] DiagramSource
    {
        get => _diagramSource;
        set
        {
            var result = _diagramSource?.Equals(value);
            _diagramSource = value;
            OnPropertyChanged(nameof(DiagramSource));
        }
    }

    public async Task BubbleSort()
    {
        await Task.Run(() =>
        {
            int num = DiagramSource.Length;
            for (int i = 0; i < num - 1; i++)
            {
                for (int j = 0; j < num - i - 1; j++)
                {
                    var j1 = j;
                    if (DiagramSource[j1] > DiagramSource[j1 + 1])
                    {
                        (DiagramSource[j1], DiagramSource[j1 + 1]) = (DiagramSource[j1 + 1], DiagramSource[j1]);
                        DiagramSource = DeepCopy(DiagramSource);
                        OnPropertyChanged("DiagramSource");
                        Thread.Sleep(700);
                    }
                }
            }
        });
    }

    private int[] DeepCopy(int[] arr)
    {
        var copyArr = new int[arr.Length];

        for (int i = 0; i < arr.Length; i++)
            copyArr[i] = arr[i];
        return copyArr;
    }
}