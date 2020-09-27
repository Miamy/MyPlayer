using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyPlayer.ViewModels
{
    public class ArtistModel : SelectionViewModel
    {
        private IArtist _artist;

        private int _count = -1;
        public int Count
        {
            get
            {
                if (_count == -1)
                {
                    _count = Albums.Sum(album => album.Count);
                }
                return _count;
            }
        }

        public IEnumerable<AlbumModel> Albums { get; private set; }
        public string Name => _artist.Name;
        public int ArtistHeight => Albums.Sum(album => album.AlbumHeight) + HeaderHeight;

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                foreach (var album in Albums)
                {
                    album.IsSelected = value;
                }
            }
        }

        public ArtistModel(IArtist artist)
        {
            _artist = artist;
            Albums = _artist.Albums.Select(album => new AlbumModel(album));
        }
    }
}
