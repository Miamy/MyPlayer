using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class AlbumModel : List<ISong>
    {
        public IAlbum Album { get; private set; }

        public AlbumModel(IAlbum album, IList<ISong> songs) : base(songs)
        {
            Album = album;
        }
    }
}
