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
    public void Change(int index, Color color);

    /// <summary>
    /// Анимация заливки выбранным цветом всех не закрашенных этим цветом элементов.
    /// </summary>
    /// <param name="cancel"></param>
    /// <param name="color"></param>
    /// <param name="delay">Задержка отрисовки цвета</param>
    /// <returns></returns>
    public Task FillAllWithAnimation(CancellationToken cancel, Color color, int delay = 50);

    /// <summary>
    /// Уведомляет об изменении списка элементов.
    /// </summary>
    public void ReplacementNotify();

}