using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Services.SubModules;

public static class CollectionCreator
{
    public static ObservableCollection<DiagramItem> GetCollection(int elementsCount = 20, int heightFactor = 25, int width = 50)
    {
        var items = new ObservableCollection<DiagramItem>();
        for (int i = 1; i < elementsCount + 1; i++)
            items.Add(new DiagramItem
            {
                Value = i,
                Height = i * heightFactor,
                Width = width,
                Color = Brushes.White,
            });
        return items;
    }
}