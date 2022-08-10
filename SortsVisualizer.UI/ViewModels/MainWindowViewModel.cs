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

    private bool CanStartSortingCommandExecute(object o) => _state is AppStates.NothingToDo;

    public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();

    private async void OnStartSortingCommandExecuted(object o)
    {
        _state = AppStates.Sorting;
        await SorterService.StartAsync(SelectedSort, DiagramSource, 80);
        _state = AppStates.NothingToDo;
        RaiseCanExecuteChanged();
    }

    #endregion


    #region Shuffle

    public ICommand ShuffleCommand { get; }

    private bool CanShuffleCommandExecute(object o) => _state is AppStates.NothingToDo;

    private void OnShuffleCommandExecuted(object o)
    {
        _state = AppStates.Shuffling;
        DiagramSourceService.Shuffle();
        _state = AppStates.NothingToDo;
    }

    #endregion

    #region Stop

    public ICommand StopSortingCommand { get; }

    private bool CanStopSortingCommandExecute(object o) => _state is AppStates.Sorting;

    private void OnStopSortingCommandExecuted(object o)
    {
        SorterService.Stop();
        _state = AppStates.NothingToDo;
    }

    #endregion

    #endregion

    #region Fields

    private AppStates _state;
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
        _state = AppStates.NothingToDo;

        DiagramSourceService = new DiagramSourcesService(OnCollectionChanged);
        DiagramSource = DiagramSourceService.Items;
        DiagramSource.CollectionChanged += MyItemsSource_CollectionChanged;
        SorterService = new SorterService(DiagramSourceService, UpdateStatistics);

        StartSortingCommand = new LambdaCommand(OnStartSortingCommandExecuted, CanStartSortingCommandExecute);
        StopSortingCommand = new LambdaCommand(OnStopSortingCommandExecuted, CanStopSortingCommandExecute);
        ShuffleCommand = new LambdaCommand(OnShuffleCommandExecuted, CanShuffleCommandExecute);
    }

    void MyItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnCollectionChanged(e.Action);
    }

}