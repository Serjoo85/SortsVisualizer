using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SortsVisualizer.UI.Controls;

/// <summary>
/// Interaction logic for ArrayVisualizer.xaml
/// </summary>
public partial class ArrayVisualizer : UserControl
{
    public ArrayVisualizer()
    {
        InitializeComponent();
    }

    private static async void CreateDiagram(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var userControl = d as ArrayVisualizer;
        var viewArray = userControl!.ViewArray;
        var array = userControl!.DiagramSource;

        await Task.Run(() =>
        {
            viewArray.Dispatcher.Invoke(() => viewArray.Children.Clear());
            foreach (Rectangle rect in array)
            {
                viewArray.Dispatcher.Invoke(() => viewArray.Children.Add(rect));
            }
        });
    }

    public Rectangle[] DiagramSource
    {
        get => (Rectangle[])GetValue(DiagramSourceProperty);
        set => SetValue(DiagramSourceProperty, value);
    }

    public static readonly DependencyProperty DiagramSourceProperty = DependencyProperty.Register
    (
        nameof(DiagramSource),
        typeof(Rectangle[]),
        typeof(ArrayVisualizer),
        new PropertyMetadata(null, CreateDiagram)
        );
}