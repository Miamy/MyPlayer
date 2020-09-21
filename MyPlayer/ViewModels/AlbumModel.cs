using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class AlbumModel : List<ISong>
    {
        public string Name { get; private set; }

        public AlbumModel(string name, IList<ISong> songs) : base(songs)
        {
            Name = name;
        }
    }
}
