using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Artist : IArtist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<IMediaBase> Children { get; set; }

        public int Count => Children.Sum(album => album.Count);
        public Artist(string name)
        {
            Name = name;
            Children = new List<IMediaBase>();
        }


        public void AddAlbum(IAlbum album)
        {
            if (Children.IndexOf(album) != -1)
            {
                return;
            }
            Children.Add(album);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
