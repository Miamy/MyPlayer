using MyPlayer.Models.Interfaces;
using System.Collections.Generic;

namespace MyPlayer.ViewModels
{
    public class AlbumModel 
    {
        private IAlbum _album;
        public int HeaderHeight { get; set; } = 40;
        public int ItemHeight { get; set; } = 30;

        public IArtist Artist => _album.Artist;
        public IList<ISong> Songs => _album.Songs;

        public string Name => _album.Name;
        public int Year => _album.Year;
        public int Count => Songs.Count;

        public int Height => Count * (ItemHeight + 2) + HeaderHeight;


        public AlbumModel(IAlbum album)
        {
            _album = album;
        }
    }
}
