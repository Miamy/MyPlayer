using System;
using System.Collections.Generic;

namespace MyPlayer.Models.Interfaces
{
    public interface IAlbum : IMediaBase
    {
        IArtist Artist { get; set; }
        IPathElement Container { get; set; }
        TimeSpan Duration { get; set; }

        int Year { get; set; }

        IGraphicFile Cover { get; set; }

        void AddSong(ISong song);
    }
}
