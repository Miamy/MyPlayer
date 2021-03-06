﻿using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MyPlayer.Models.Interfaces
{
    public interface IQueueViewModel : INotifyPropertyChanged
    {
        IList<VisualObject<IMediaBase>> Artists { get; }

        IQueue Queue { get; set; }
        bool ExpandAlbums { get; set; }
        bool ExpandSongs { get; set; }
        bool AllSelected { get; set; }
        string SearchText { get; set; }
        bool SearchIsEmpty { get; }
        bool SearchIsNotEmpty { get; }

        ICommand PlayTappedCommand { get; set; }

    }
}