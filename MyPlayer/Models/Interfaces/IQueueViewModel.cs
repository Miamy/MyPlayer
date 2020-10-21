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
        IList<VisualObject<ISong>> Flattened { get; }

        bool ShowAlbums { get; set; }
        bool ShowSongs { get; set; }

        VisualObject<ISong> Next(VisualObject<ISong> song);
        VisualObject<ISong> Prev(VisualObject<ISong> song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        VisualObject<ISong> GetDefault();
        void SwitchLoopType();
        void AddFromRoot(string path);
    }
}