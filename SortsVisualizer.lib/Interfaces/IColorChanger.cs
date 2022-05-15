using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Анимация заливки зелёным не зелёных элементов.
    /// </summary>
    /// <param name="collection">Ссылка на коллекцию</param>
    /// <param name="cancel">Токен завершения</param>
    /// <returns></returns>
    public Task FinishPaint(ObservableCollection<DiagramItem> collection, CancellationToken cancel);

    public Task FillAll(ObservableCollection<DiagramItem> collection, CancellationToken cancel);

    public void ReplacementNotify();

}