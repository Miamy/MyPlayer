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
        IList<IAlbum> Albums { get; set; }
        IList<IMediaBase> Artists { get; set; }

        //void Add(IPathElement item);
        //void AddFromRoot(string path);
        //void Remove(IPathElement item);

        void FillFromRoot(string path);

        void Clear();

        ISong Next(ISong song);
        ISong Prev(ISong song);

        LoopType LoopType { get; set; }
        ISong Current { get; set; }
        ISong GetDefault();
        void SwitchLoopType();
    }
}
