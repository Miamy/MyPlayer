using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyPlayer.ViewModels
{
    public class ArtistModel
    {
        public IArtist Artist { get; set; }

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

        public IList<IAlbum> Albums => Artist.Albums;
    }
}
