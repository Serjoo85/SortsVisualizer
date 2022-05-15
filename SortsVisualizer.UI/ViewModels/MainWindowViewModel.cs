using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MailSender.lib.Commands;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services;
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

    #region Commands

    #region Start

    public ICommand StartSortingCommand { get; }

    private bool CanStartSortingCommandExecute(object o) => true;

    private async void OnStartSortingCommandExecuted(object o)
    {
        await SorterService.StartAsync(SortType.Bubble, DiagramSource);
    }

    #endregion

    #region Stop

    public ICommand StopSortingCommand { get; }

    private bool CanStopSortingCommandExecute(object o) => true;

    private void OnStopSortingCommandExecuted(object o)
    {
        SorterService.Stop();
    }

    #endregion

    #endregion

    #region Properties

    public ObservableCollection<DiagramItem> DiagramSource { get; set; }
    public ISorterService SorterService { get; }

    public string[] SortersTypes => SorterService.GetSortersTypes();

    #endregion

    public MainWindowViewModel()
    {
        DiagramSource = TestData.ObservableCollection;
        SorterService = new SorterService(OnCollectionChanged);
        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
        StopSortingCommand = new LambdaCommand(OnStopSortingCommandExecuted, CanStopSortingCommandExecute);
    }
}