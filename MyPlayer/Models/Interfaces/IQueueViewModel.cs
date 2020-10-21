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
        IList<VisualObject<IMediaBase>> Flattened { get; }

        bool ShowAlbums { get; set; }
        bool ShowSongs { get; set; }

        VisualObject<ISong> Next(VisualObject<IMediaBase> song);
        VisualObject<ISong> Prev(VisualObject<IMediaBase> song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        VisualObject<IMediaBase> GetDefault();
        void SwitchLoopType();
        void AddFromRoot(string path);
    }
}