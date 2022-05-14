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
        await BubbleSortObservableCollectionAsync();
    }

    #endregion

    public ObservableCollection<DiagramItem> DiagramSource { get; set; }

    public async Task BubbleSortObservableCollectionAsync()
    {
        int num = DiagramSource.Count;
        for (int i = 0; i < num - 1; i++)
        {
            var hasSwap = false;
            for (int j = 0; j < num - i - 1; j++)
            {
                // Текущий элемент.
                var j1 = j;
                // Закрашиваем текущий элемент с перестановкой.
                if (DiagramSource[j1].Value > DiagramSource[j1 + 1].Value)
                {
                    hasSwap = true;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (j1 > 0)
                            ChangeColor(DiagramSource, j1 - 1, Colors.White);
                        ChangeColor(DiagramSource, j1, Colors.Orange);
                        (DiagramSource[j1], DiagramSource[j1 + 1]) = (DiagramSource[j1 + 1], DiagramSource[j1]);
                    });
                }
                // Закрашиваем текущий элемент без перестановки.
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (j1 > 0)
                            ChangeColor(DiagramSource, j1 - 1, Colors.White);
                        ChangeColor(DiagramSource, j1, Colors.Orange);
                    });

                }
                OnCollectionChanged(NotifyCollectionChangedAction.Replace);
                await Task.Delay(100);
            }

            // Красим в белый последний закрашенный прямоугольник.
            Application.Current.Dispatcher.Invoke(() => ChangeColor(DiagramSource, num - i - 2, Colors.White));
            OnCollectionChanged(NotifyCollectionChangedAction.Replace);
            // Проход без замены признак отсортированной последовательности.
            if (hasSwap == false) return;
        }

        void ChangeColor(ObservableCollection<DiagramItem> collection, int index, Color color)
        {
            var newItem = collection[index];
            newItem.Color = new SolidColorBrush(color);
            collection[index] = newItem;
        }
    }
}