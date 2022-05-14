using System.Windows.Media;

namespace SortsVisualizer.lib.Models;

/// <summary>
/// Элемент диаграммы
/// </summary>
/// <remarks>
/// В случае использования класса, при изменении свойства Color
/// в wpf не произойдёт закраска элемента после OnCollectionChanged.
/// Только после замены ссылки элемента DiagramItem.
/// При использовании структуры изменение свойства Color возможно только
/// в случае замены DiagramItem на копию с изменённым свойством.
/// </remarks>
public struct DiagramItem
{
    public int Value { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public SolidColorBrush Color { get; set; }
}