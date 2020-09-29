using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyPlayer.ViewModels
{
    public class AlbumModel : SelectionViewModel
    {
        private IAlbum _album;
        private QueueViewModel _owner;

        public IArtist Artist => _album.Artist;
        public IList<SongModel> Songs { get; private set; }

        public string Name => _album.Name;
        public int Year => _album.Year;

        private int _count = -1;
        public int Count
        {
            get
            {
                if (_count == -1)
                {
                    _count = Songs.Count();
                }
                return _count;
            }
        }

        public string FullName
        {
            get
            {
                var res = _album.Name;
                if (Year != 0)
                {
                    res = Year + " - " + res;
                }
                return res;
            }
        }

        public int AlbumHeight
        {
            get
            {
                if (_owner.ShowSongs)
                {
                    return Count * ItemHeight;
                }
                return 0;
            }
        }

        public int AlbumTotalHeight => AlbumHeight + HeaderHeight;

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                foreach (var song in Songs)
                {
                    song.IsSelected = value;
                }
            }
        }

        public AlbumModel(IAlbum album, QueueViewModel owner)
        {
            _album = album;
            _owner = owner;
            Songs = _album.Songs.Select(song => new SongModel(song)).ToList();
        }
    }
}
