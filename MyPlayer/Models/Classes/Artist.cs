using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Artist : MediaBase, IArtist
    {
        //public override int Count { get; protected set; }

        public Artist(string name, string container) : base(name, container)
        {
            Children = new List<IMediaBase>();
            Count = 0;
        }


        public void AddAlbum(IAlbum album)
        {
            if (Children.IndexOf(album) != -1)
            {
                return;
            }
            Children.Add(album);
          //  Count += album.Count;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
