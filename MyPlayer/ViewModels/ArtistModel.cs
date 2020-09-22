using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{

    public class ArtistModel : List<IAlbum>
    {
        public IArtist Artist { get; private set; }

        public ArtistModel(IArtist artist, IList<IAlbum> albums) : base(albums)
        {
            Artist = artist;
        }
    }
}
