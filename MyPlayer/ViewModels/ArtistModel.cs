using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyPlayer.ViewModels
{
    public class ArtistModel : SelectionViewModel
    {
        private IArtist _artist;
        private QueueViewModel _owner;

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

        public IList<AlbumModel> Albums { get; private set; }
        public string Name => _artist.Name;
        public int DiscographyHeight
        {
            get
            {
                if (_owner.ShowAlbums)
                {
                    return Albums.Sum(album => album.AlbumTotalHeight);
                }
                return 0;
            }
        }

        public int DiscographyTotalHeight => DiscographyHeight + HeaderHeight;
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

        public ArtistModel(IArtist artist, QueueViewModel owner)
        {
            _artist = artist;
            _owner = owner;
            Albums = _artist.Albums.Select(album => new AlbumModel(album, _owner)).ToList();
        }
    }
}
