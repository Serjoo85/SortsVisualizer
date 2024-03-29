﻿using System.Collections.ObjectModel;
using SortsVisualizer.lib.Models;

namespace SortsVisualizer.lib.Models.Interfaces;

public interface ISorterStrategy
{
    Task StartAsync(ObservableCollection<DiagramItem> collection, int delay);
    void Stop();
}