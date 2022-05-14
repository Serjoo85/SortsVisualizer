using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MailSender.lib.Commands;
using SortsVisualizer.lib.Models;
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
        DiagramSource = TestData.ObservableCollection;
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

    public ObservableCollection<DiagramItem> DiagramSource { get; set; }

    public async Task BubbleSortObservableCollection()
    {
        await Task.Run(() =>
        {
            int num = DiagramSource.Count;
            for (int i = 0; i < num - 1; i++)
            {
                var hasSwap = false;
                for (int j = 0; j < num - 1; j++)
                {
                    var j1 = j;
                    if (DiagramSource[j1].Value > DiagramSource[j1 + 1].Value)
                    {
                        hasSwap = true;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (j1 > 0)
                                DiagramSource[j1 - 1] = ChangeColor(DiagramSource[j1 - 1], Colors.White);
                            DiagramSource[j1] = ChangeColor(DiagramSource[j1], Colors.Orange);
                            (DiagramSource[j1], DiagramSource[j1 + 1]) = (DiagramSource[j1 + 1], DiagramSource[j1]);
                        });
                        OnCollectionChanged(NotifyCollectionChangedAction.Replace);
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (j1 > 0)
                                DiagramSource[j1 - 1] = ChangeColor(DiagramSource[j1 - 1], Colors.White);
                            DiagramSource[j1] = ChangeColor(DiagramSource[j1], Colors.Orange);
                        });
                        OnCollectionChanged(NotifyCollectionChangedAction.Replace);
                    }
                    Thread.Sleep(50);
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    DiagramSource[num - 1] = ChangeColor(DiagramSource[num - 1], Colors.White);
                    DiagramSource[num - 2] = ChangeColor(DiagramSource[num - 2], Colors.White);
                });
                if (hasSwap == false) return;
            }
        });

        DiagramItem ChangeColor(DiagramItem item, Color color)
        {
            var newItem = item;
            newItem.Color = new SolidColorBrush(color);
            return newItem;
        }
    }
}