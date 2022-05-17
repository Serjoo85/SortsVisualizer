using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using SortsVisualizer.lib.Commands;
using SortsVisualizer.lib.Enums;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services;

namespace SortsVisualizer.UI.ViewModels;

public class MainWindowViewModel : INotifyCollectionChanged, INotifyPropertyChanged
{
    #region ICollectionChanged

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedAction notify)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(notify));
    }

    #endregion

    #region IPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Commands

    #region Start

    public ICommand StartSortingCommand { get; }

    private bool CanStartSortingCommandExecute(object o) => _state is ProcessStates.NothingToDo;

    public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();

    private async void OnStartSortingCommandExecuted(object o)
    {
        _state = ProcessStates.Sorting;
        await SorterService.StartAsync(SelectedSort, CountSteps);
        _state = ProcessStates.NothingToDo;
        RaiseCanExecuteChanged();
    }

    #endregion


    #region Shuffle

    public ICommand ShuffleCommand { get; }

    private bool CanShuffleCommandExecute(object o) => _state is ProcessStates.NothingToDo;

    private void OnShuffleCommandExecuted(object o)
    {
        _state = ProcessStates.Shuffling;
        DiagramSourceService.Shuffle();
        _state = ProcessStates.NothingToDo;
    }

    #endregion

    #region Stop

    public ICommand StopSortingCommand { get; }

    private bool CanStopSortingCommandExecute(object o) => _state is ProcessStates.Sorting;

    private void OnStopSortingCommandExecuted(object o)
    {
        SorterService.Stop();
        _state = ProcessStates.NothingToDo;
    }

    #endregion

    #endregion

    #region Fields

    private ProcessStates _state;
    private SortType _selectedSort;
    private int _numberOfStep;

    #endregion

    #region Properties

    public int NumberOfStep
    {
        get => _numberOfStep;
        set
        {
            _numberOfStep = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DiagramItem> DiagramSource { get; set; }
    public ISorterService SorterService { get; }
    public IDiagramSourceService DiagramSourceService { get;}
    public string[] SortersTypes => SorterService.GetSortersTypes();

    public SortType SelectedSort
    {
        get => _selectedSort;
        set
        {
            _selectedSort = value;
            OnPropertyChanged(nameof(SelectedSort));
        }
    }

    #endregion

    #region Methods

    private void CountSteps(int quantity)
    {
        NumberOfStep = quantity;
    }

    #endregion

    public MainWindowViewModel()
    {
        _state = ProcessStates.NothingToDo;

        DiagramSourceService = new DiagramSourcesService(OnCollectionChanged);
        DiagramSource = DiagramSourceService.Items;
        SorterService = new SorterService(DiagramSourceService, DiagramSource);

        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
        StopSortingCommand = new LambdaCommand(OnStopSortingCommandExecuted, CanStopSortingCommandExecute);
        ShuffleCommand = new LambdaCommand(OnShuffleCommandExecuted, CanShuffleCommandExecute);
    }


}