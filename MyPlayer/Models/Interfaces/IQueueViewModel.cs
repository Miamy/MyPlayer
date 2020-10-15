using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyPlayer.Models.Interfaces
{
    public interface IQueueViewModel : IId
    {
        IReadOnlyCollection<VisualObject<IArtist>> Artists { get; }

        ISong Next(ISong song);
        ISong Prev(ISong song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        ISong GetDefault();
        void SwitchLoopType();
        void AddFromRoot(string path);
    }
}