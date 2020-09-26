using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyPlayer.ViewModels
{
    public class AlbumModel : SelectionViewModel
    {
        private IAlbum _album;

        public IArtist Artist => _album.Artist;
        public IList<SongModel> Songs { get; private set; }

        public string Name => _album.Name;
        public int Year => _album.Year;
        public int Count => Songs.Count;

        public int Height => Count * (ItemHeight) + HeaderHeight;


        public AlbumModel(IAlbum album)
        {
            _album = album;
            Songs = _album.Songs.Select(song => new SongModel(1, song)).ToList();
        }
    }
}
