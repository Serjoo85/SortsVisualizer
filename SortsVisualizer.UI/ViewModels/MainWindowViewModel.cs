using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MailSender.lib.Commands;
using SortsVisualizer.UI.Data;

namespace SortsVisualizer.UI.ViewModels;

public class MainWindowViewModel : INotifyCollectionChanged
{
    #region ICollectionChanged

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction notify)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(notify));
    }

    #endregion
    
    public MainWindowViewModel()
    {
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
}