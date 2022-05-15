using System.Collections.ObjectModel;
using SortsVisualizer.lib.Interfaces;
using SortsVisualizer.lib.Models.Base;

namespace SortsVisualizer.lib.Models;

public class BubbleOptimizedSorting : BaseSorting
{
    public BubbleOptimizedSorting(IColorChanger colorChanger) : base(colorChanger)
    {
    }

    protected override async Task SortAsync(ObservableCollection<DiagramItem> collection, CancellationToken cancel)
    {
        int n = 10;

        //Keep looping until list is sorted
        do
        {    //This variable is used to store the
             //position of the last swap
            int sw = 0;

            //Loop through all elements in the list
            for (int i = 0; i < n - 1; i++)
            {
                //If the current pair of elements is 
                //not in order then swap them and update 
                //the position of the swap 
                if (collection[i].Value > collection[i + 1].Value)
                {
                    //Swap
                    (collection[i], collection[i + 1]) = (collection[i + 1], collection[i]);
                    ColorChanger.ReplacementNotify();
                    //Save swap position
                    sw = i + 1;

                    await Task.Delay(150, cancel);
                }
            }

            //We do not need to visit all elements
            //we only need to go as far as the last swap
            //so we update (n)
            n = sw;
        }

        //Once n = 1 then the whole list is sorted
        while (n > 1);
    }



}