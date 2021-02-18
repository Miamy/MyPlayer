using MyPlayer.Models.Interfaces;
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
        //IList<VisualObject<IMediaBase>> Flattened { get; }
        //IList<ISong> Songs { get; }

        bool ExpandAlbums { get; set; }
        bool ExpandSongs { get; set; }
        bool AllSelected { get; set; }
        string SearchText { get; set; }
        bool SearchIsEmpty { get; }

        ICommand PlayTappedCommand { get; set; }

    }
}