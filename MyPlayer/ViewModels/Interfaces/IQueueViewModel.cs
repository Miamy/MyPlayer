using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyPlayer.Models.Interfaces
{
    public interface IQueueViewModel 
    {
        IReadOnlyCollection<VisualObject<IMediaBase>> Artists { get; }
        //IList<VisualObject<IMediaBase>> Flattened { get; }
        IList<ISong> Songs { get; }

        bool ShowAlbums { get; set; }
        bool ShowSongs { get; set; }

        ISong Next(ISong song);
        ISong Prev(ISong song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        ISong GetDefault();
        void SwitchLoopType();
        void AddFromRoot(string path);
    }
}