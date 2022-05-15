using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using SortsVisualizer.lib.Interfaces;

namespace SortsVisualizer.lib.Models;

public class BubbleSorting : ISorterStrategy
{
    private CancellationTokenSource _cts = null!;
    private readonly Action<NotifyCollectionChangedAction> _onCollectionChanged;

    public BubbleSorting(Action<NotifyCollectionChangedAction> onCollectionChanged)
    {
        _onCollectionChanged = onCollectionChanged;
    }

    public async Task StartAsync(ObservableCollection<DiagramItem> collection)
    {
        _cts = new CancellationTokenSource();
        try
        {
            await SortAsync(collection, _cts.Token);
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine("Action was interrupted by user.");
        }
        finally
        {
            _cts.Dispose();
        }
    }

    private async Task SortAsync(
        ObservableCollection<DiagramItem> collection,
        CancellationToken cancel = default)
    {
        int num = collection.Count;
        for (int i = 0; i < num - 1; i++)
        {
            var hasSwap = false;
            for (int j = 0; j < num - i - 1; j++)
            {
                // j - индекс текущего элемента.

                // Закрашиваем текущий элемент с перестановкой.
                if (collection[j].Value > collection[j + 1].Value)
                {
                    hasSwap = true;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (j > 0)
                            ChangeColor(j - 1, Colors.White);
                        ChangeColor(j, Colors.Orange);
                        (collection[j], collection[j + 1]) = (collection[j + 1], collection[j]);
                    });
                }
                // Закрашиваем текущий элемент без перестановки.
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (j > 0)
                            ChangeColor(j - 1, Colors.White);
                        ChangeColor(j, Colors.Orange);
                    });

                }

                _onCollectionChanged(NotifyCollectionChangedAction.Replace);
                await Task.Delay(100, cancel);
            }

            // Красим в белый последний закрашенный прямоугольник.
            Application.Current.Dispatcher.Invoke(() =>
            {
                ChangeColor(num - i - 1, Colors.White);
                ChangeColor(num - i - 2, Colors.White);
            });
            _onCollectionChanged(NotifyCollectionChangedAction.Replace);
            // Проход без замены признак отсортированной последовательности.
            if (hasSwap == false) return;
        }

        void ChangeColor(int index, Color color)
        {
            var newItem = collection[index];
            newItem.Color = new SolidColorBrush(color);
            collection[index] = newItem;
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
    }
    //    int n = 10;

    //        //Keep looping until list is sorted
    //        do
    //    {    //This variable is used to store the
    //        //position of the last swap
    //        int sw = 0;

    //        //Loop through all elements in the list
    //        for (int i = 0; i<n - 1; i++) 
    //        { 
    //            //If the current pair of elements is 
    //            //not in order then swap them and update 
    //            //the position of the swap 
    //            if (A[i] > A[i + 1])
    //            {
    //                //Swap
    //                int temp = A[i];
    //                A[i] = A[i + 1];
    //                A[i + 1] = temp;

    //                //Save swap position
    //                sw = i + 1;
    //            }
    //        }

    //        //We do not need to visit all elements
    //        //we only need to go as far as the last swap
    //        //so we update (n)
    //        n = sw;
    //    }

    ////Once n = 1 then the whole list is sorted
    //while (n > 1) ;
    //}

}