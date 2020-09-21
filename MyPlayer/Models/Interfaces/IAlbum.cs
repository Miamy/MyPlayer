using System;
using System.Collections.Generic;

namespace MyPlayer.Models.Interfaces
{
    public interface IAlbum : IMediaBase
    {
        string Name { get; set; }
        IArtist Artist { get; set; }
        IPathElement Container { get; set; }
        TimeSpan Duration { get; set; }

        IList<ISong> Songs { get; set; }

        int Year { get; set; }

        //MusicFileType Type { get; set; }

        void AddSong(ISong song);
    }
}
