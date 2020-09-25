using MyPlayer.Models.Interfaces;
using System.Collections.Generic;

namespace MyPlayer.ViewModels
{
    public class AlbumModel 
    {
        public int HeaderHeight { get; set; } = 40;
        public int ItemHeight { get; set; } = 30;

        public IAlbum Album { get; set; }
        public int Count => Songs.Count;
        public List<ISong> Songs { get; set; }

        public int Height => Count * (ItemHeight + 2) + HeaderHeight;
    }
}
