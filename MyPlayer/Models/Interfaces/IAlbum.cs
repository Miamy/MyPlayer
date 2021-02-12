using System;
using System.Collections.Generic;

namespace MyPlayer.Models.Interfaces
{
    public interface IAlbum : IMediaBase
    {
        IArtist Artist { get; set; }

        int Year { get; set; }

        string Cover { get; set; }

        void AddSong(ISong song);
    }
}
