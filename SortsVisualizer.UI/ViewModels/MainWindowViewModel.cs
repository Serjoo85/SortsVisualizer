using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SortsVisualizer.UI.Data;

namespace SortsVisualizer.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Base functionality INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged = null!;

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
        DiagramSource = CreateRectangleArray(TestData.array);
    }

    private Rectangle[] _diagramSource = null!;
    public Rectangle[] DiagramSource
    {
        get => _diagramSource;
        set
        {
            _diagramSource = value;
            OnPropertyChanged(nameof(DiagramSource));
        }
    }


    public Rectangle[] CreateRectangleArray(int[] array)
    {
        var rectangles = new List<Rectangle>();
        var min = array.Min();
        var max = array.Max();
        var width = 1000 / array.Length;
        var heightFactor = 500 / max;

        foreach (var value in array)
        {
            rectangles.Add(
                CreateRectangle(value * heightFactor, width));
        }

        return rectangles.ToArray();
    }

    private Rectangle CreateRectangle(int height, int width)
    {
        var rectangle = new System.Windows.Shapes.Rectangle
        {
            Width = width,
            Height = height,
            Stroke = Brushes.Black,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        return rectangle;
    }
}