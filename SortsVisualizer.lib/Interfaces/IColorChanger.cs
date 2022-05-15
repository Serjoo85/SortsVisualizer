using System.Collections.ObjectModel;
using System.Windows.Media;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Interfaces;

public interface IColorChanger
{
    /// <summary>
    /// Замена цвета заливки элемента коллекции
    /// </summary>
    /// <param name="index">Индекс элемента</param>
    /// <param name="color">Новый цвет</param>
    /// <param name="collection">Ссылка на коллекцию</param>
    public void Change(int index, Color color, ObservableCollection<DiagramItem> collection);
    
    /// <summary>
    /// Анимация заливки выбранным цветом всех не закрашенных этим цветом элементов. 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="cancel"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public Task FillAllWithAnimation(ObservableCollection<DiagramItem> collection, CancellationToken cancel, Color color);

    public void ReplacementNotify();

}