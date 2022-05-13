using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace SortsVisualizer.UI.Controls;

/// <summary>
/// Interaction logic for ArrayVisualizer.xaml
/// </summary>
public partial class ArrayVisualizer : UserControl, INotifyPropertyChanged
{
    private static int _heightFactor;

    public ArrayVisualizer()
    {
        InitializeComponent();
    }

    #region Base functionality INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

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

    private static void CreateDiagram(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var userControl = d as ArrayVisualizer;
        var viewArray = userControl!.ViewArray;
        var array = userControl!.DiagramSource;
        if (viewArray.Children.Count > 0)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var i1 = i;
                    viewArray.Dispatcher.Invoke(() => ((Rectangle)viewArray.Children[i1]).Height = array[i1] * _heightFactor);
                }
            });
            return;
        }
        var max = array.Max();
        int width = 1000 / array.Length;
        _heightFactor = 500 / max;

        Task.Run(() =>
        {
            viewArray.Dispatcher.Invoke(() => viewArray.Children.Clear());
            foreach (var height in array)
            {
                var rect = CreateRectangle(height * _heightFactor, width);
                viewArray.Dispatcher.Invoke(() => viewArray.Children.Add(rect));
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            }
        });
    }

    public int[] DiagramSource
    {
        get => (int[])GetValue(DiagramSourceProperty);
        set => SetValue(DiagramSourceProperty, value);
    }

    public static readonly DependencyProperty DiagramSourceProperty = DependencyProperty.Register
    (
        nameof(DiagramSource),
        typeof(int[]),
        typeof(ArrayVisualizer),
        new PropertyMetadata(null, CreateDiagram)
        );

    private static Rectangle CreateRectangle(int height, int width)
    {
        Rectangle rectangle = null!;
        Application.Current.Dispatcher.Invoke(() =>
        {
            rectangle = new System.Windows.Shapes.Rectangle
            {
                Width = width,
                Height = height,
                Stroke = Brushes.Black,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
        });

        return rectangle!;
    }
}