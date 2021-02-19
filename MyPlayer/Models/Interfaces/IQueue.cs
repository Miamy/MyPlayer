using MyPlayer.Models.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IQueue : INotifyPropertyChanged
    {
        IList<ISong> Songs { get; }
        IList<IMediaBase> Artists { get; set; }

        void Fill(string path);

        void Clear();

        ISong Next(ISong song);
        ISong Prev(ISong song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        void SwitchLoopType();
    }
}
