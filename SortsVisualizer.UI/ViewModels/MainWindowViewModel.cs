using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MailSender.lib.Commands;
using SortsVisualizer.lib.Services;
using SortsVisualizer.UI.Data;

namespace SortsVisualizer.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged, INotifyCollectionChanged
{
    #region ICollectionChanged

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction notify)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(notify));
    }

    #endregion

    #region INotifyPropertyChanged

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
        DiagramSourceArray = TestData.Array;
        DiagramSourceObservableCollection = TestData.ObservableCollection;
        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
    }

    #region Commands

    public ICommand StartSortingCommand { get; }

    private bool CanStartSortingCommandExecute(object o) => true;

    private async void OnStartSortingCommandExecuted(object o)
    {
        await BubbleSortObservableCollection();
    }

    #endregion

    public ObservableCollection<int> DiagramSourceObservableCollection { get; set; }

    private int[] _diagramSourceArray;
  

    public int[] DiagramSourceArray
    {
        get => _diagramSourceArray;
        set
        {
            _diagramSourceArray = value;
            OnPropertyChanged(nameof(DiagramSourceArray));
        }
    }

    public async Task BubbleSortObservableCollection()
    {
        await Task.Run(() =>
        {
            int num = DiagramSourceObservableCollection.Count;
            for (int i = 0; i < num - 1; i++)
            {
                for (int j = 0; j < num - i - 1; j++)
                {
                    var j1 = j;
                    if (DiagramSourceObservableCollection[j1] > DiagramSourceObservableCollection[j1 + 1])
                    {
                        Application.Current.Dispatcher.Invoke(() => (DiagramSourceObservableCollection[j1], DiagramSourceObservableCollection[j1 + 1]) = (DiagramSourceObservableCollection[j1 + 1], DiagramSourceObservableCollection[j1]));
                        OnCollectionChanged(NotifyCollectionChangedAction.Replace);
                        Thread.Sleep(500);
                    }
                }
            }
        });
    }

    public async Task BubbleSortArray()
    {
        await Task.Run(() =>
        {
            int num = DiagramSourceArray.Length;
            for (int i = 0; i < num - 1; i++)
            {
                for (int j = 0; j < num - i - 1; j++)
                {
                    var j1 = j;
                    if (DiagramSourceArray[j1] > DiagramSourceArray[j1 + 1])
                    {
                        (DiagramSourceArray[j1], DiagramSourceArray[j1 + 1]) = (DiagramSourceArray[j1 + 1], DiagramSourceArray[j1]);
                        DiagramSourceArray = DeepCopy(DiagramSourceArray);
                        OnPropertyChanged(nameof(DiagramSourceArray));
                        Thread.Sleep(500);
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