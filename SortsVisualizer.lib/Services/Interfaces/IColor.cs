using System.Windows.Media;

namespace SortsVisualizer.lib.Services.Interfaces;

public interface IColor
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
    public Task MakeLadderAnimation(CancellationToken cancel, Color color, int delay = 50);
}