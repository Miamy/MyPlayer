using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Album : IAlbum
    {
        private string _name;
        public int Id { get; set; }
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
                    //_name = _name.Substring(6, _name.Length - 6);
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
        public IList<IMediaBase> Children { get; set; }

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
        public IGraphicFile Cover { get; set; }

        public Album(string name)
        {
            Name = name;
            Children = new List<IMediaBase>();
        }

        public int Count => Children.Count;
        private void Clear()
        {
            Cover = null;
            Year = 0;
            Duration = Consts.ZeroTimeSpan;
            Children.Clear();
        }

        public void AddSong(ISong song)
        {
            if (Children.IndexOf(song) != -1)
            {
                return;
            }
            Children.Add(song);
            Duration += song.Duration;

            if (Cover == null)
            {
                var dir = Path.GetDirectoryName(song.Container.FullPath);
                var files = PathScanner.GetCovers(dir);
                if (files.Length > 0)
                {
                    Cover = new GraphicFile(null, files[0]);
                }
            }
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

        public static int ExtractYear(string name)
        {
            if (name.Length < 4)
            {
                return 0;
            }
            var year = name.Substring(0, 4);
            if (int.TryParse(year, out int test))
            {
                return test;
            }
            return 0;
        }
    }
}
