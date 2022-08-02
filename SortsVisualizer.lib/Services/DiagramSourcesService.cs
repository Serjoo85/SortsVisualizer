using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SortsVisualizer.lib.Models;
using SortsVisualizer.lib.Services.Interfaces;
using SortsVisualizer.lib.Services.SubModules;

namespace SortsVisualizer.lib.Services;

public class DiagramSourcesService : IDiagramSourceService
{

    private static readonly Random Rnd = new Random();
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;
    public ColorChanger ColorChanger { get; }
    public ObservableCollection<DiagramItem> Items { get; }

    public DiagramSourcesService(Action<NotifyCollectionChangedAction> onCollectionChanged, int elementsCount = 20, int heightFactor = 25, int width = 50)
    {
        _onCollectionChanged = onCollectionChanged;
        Items = CollectionCreator.GetCollection(elementsCount, heightFactor, width);
        MixCollection(Items);
        ColorChanger = new ColorChanger(Items, _onCollectionChanged);
    }

    public void Shuffle()
    {
        MixCollection(Items);
    }

    private void MixCollection(Collection<DiagramItem> items)
    {
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 20 - 3; j++)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j + r]) = (items[j + r], items[j]);
            }

        for (int i = 0; i < 2; i++)
            for (int j = 19; j > 3; j--)
            {
                var r = Rnd.Next(1, 3);
                (items[j], items[j - r]) = (items[j - r], items[j]);
            }
    }


    public void CollectionNotify()
    {
        _onCollectionChanged.Invoke(NotifyCollectionChangedAction.Replace);
    }
}