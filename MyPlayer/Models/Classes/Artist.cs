using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Artist : IArtist
    {
        public string Name { get; set; }
        public IList<IAlbum> Albums { get; set; }

        public Artist(string name)
        {
            Name = name;
            Albums = new List<IAlbum>();
        }


        public void AddAlbum(IAlbum album)
        {
            if (Albums.IndexOf(album) != -1)
            {
                return;
            }
            Albums.Add(album);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
