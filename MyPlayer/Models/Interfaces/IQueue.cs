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

     
        LoopType LoopType { get; set; }
        IList<IAlbum> Albums { get; set; }
        IList<IArtist> Artists { get; set; }

        void Add(IMusicFile item);
        void AddRange(IEnumerable<IMusicFile> items);
        void AddRange2(IEnumerable<string> items);
        void Remove(IPathElement item);

        void Play();
        void Pause();
        void Stop();
        ISong Next(ISong song);
        ISong Prev(ISong song);

        ISong GetDefault();
        void SwitchLoopType();
    }
}
