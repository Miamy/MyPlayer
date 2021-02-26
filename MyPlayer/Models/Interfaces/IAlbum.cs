using System;
using System.Collections.Generic;

namespace MyPlayer.Models.Interfaces
{
    public interface IAlbum : IMediaBase
    {
        IArtist Artist { get; set; }

        int Year { get; set; }

        public List<string> Covers { get; set; }

        void AddSong(ISong song);
    }
}
