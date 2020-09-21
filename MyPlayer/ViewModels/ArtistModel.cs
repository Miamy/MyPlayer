using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{

    public class ArtistModel : List<IAlbum>
    {
        public string Name { get; private set; }

        public ArtistModel(string name, IList<IAlbum> albums) : base(albums)
        {
            Name = name;
        }
    }
}
