using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SortsVisualizer.lib;
using SortsVisualizer.lib.Commands;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services;
using SortsVisualizer.lib.Services.Interfaces;

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
        await SorterService.StartAsync(SelectedSort, DiagramSource, 80);
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
    private int _numberOfReplacementses;
    private int _numberOfComparisons;

    #endregion

    #region Properties

    public int NumberOfReplacements
    {
        get => _numberOfReplacementses;
        set
        {
            _numberOfReplacementses = value;
            OnPropertyChanged();
        }
    }

    public int NumberOfComparisons
    {
        get => _numberOfComparisons;
        set
        {
            _numberOfComparisons = value;
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

    private void UpdateStatistics(Statistics info)
    {
        NumberOfReplacements = info.Replacement;
        NumberOfComparisons = info.Comparison;
    }

    #endregion

    public MainWindowViewModel()
    {
        _state = ProcessStates.NothingToDo;

        DiagramSourceService = new DiagramSourcesService(OnCollectionChanged);
        DiagramSource = DiagramSourceService.Items;
        SorterService = new SorterService(DiagramSourceService, UpdateStatistics);

        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
        StopSortingCommand = new LambdaCommand(OnStopSortingCommandExecuted, CanStopSortingCommandExecute);
        ShuffleCommand = new LambdaCommand(OnShuffleCommandExecuted, CanShuffleCommandExecute);
    }


}