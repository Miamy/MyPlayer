using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Album : MediaBase, IAlbum
    {
        public string Name { get; set; }

        private IPathElement _container;

        public IPathElement Container
        {
            get => _container;
            set
            {
                _container = value;
            }
        }
        public TimeSpan Duration { get; set; }
        public IList<ISong> Songs { get; set; }

        private IArtist _artist;
        public IArtist Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                if (_artist != null)
                { 
                    _artist.AddAlbum(this);
                }
            }
        }

        public Album(string name)
        {
            Name = name;
            Songs = new List<ISong>();
        }


        private void Clear()
        {
            MediaInfo.Clear();
            Duration = TimeSpan.FromSeconds(0);
        }

        public void AddSong(ISong song)
        {
            if (Songs.IndexOf(song) != -1)
            {
                return;
            }
            Songs.Add(song);
            Duration += song.Duration;

        }

    }
}
