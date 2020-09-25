using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Album : MediaBase, IAlbum
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (_name.Length < 4)
                {
                    return;
                }
                var year = _name.Substring(0, 4);
                if (int.TryParse(year, out int test))
                {
                    Year = test;
                    _name = _name.Substring(6, _name.Length - 6);
                }
            }
        }

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

        public int Year { get; set; }

        public Album(string name)
        {
            Name = name;
            Songs = new List<ISong>();
        }

        public int Count => Songs.Count* 30 + 30;
        private void Clear()
        {
            MediaInfo.Clear();
            Duration = Consts.ZeroTimeSpan;
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

        public override string ToString()
        {
            var result = Name;
            if (Year != 0)
            {
                result = Year + " - " + result;
            }
            if (Artist != null)
            {
                result = Artist.ToString() + " / " + result;
            }
            return result;
        }
    }
}
