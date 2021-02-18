using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Album : MediaBase, IAlbum
    {
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                if (base.Name.Length < 4)
                {
                    return;
                }
                var year = base.Name.Substring(0, 4);
                if (int.TryParse(year, out int test))
                {
                    Year = test;
                }
            }
        }


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
        public string Cover { get; set; }

        public Album(string name, string container) : base(name, container)
        {
            Children = new List<IMediaBase>();

            //if (Cover == null)
            {
                var dir = Path.GetDirectoryName(container);
                var files = PathScanner.GetCovers(dir);
                if (files.Length > 0)
                {
                    Cover = files[0];
                }
            }
        }

        /*private void Clear()
        {
            Cover = null;
            Year = 0;
            Duration = Consts.ZeroTimeSpan;
            Children.Clear();
        }*/

        public void AddSong(ISong song)
        {
            if (Children.IndexOf(song) != -1)
            {
                return;
            }
            Children.Add(song);
            //Duration += song.Duration;

     
        }

        public override string ToString()
        {
            var result = Name;
            //if (Year != 0)
            //{
            //    result = Year + " - " + result;
            //}
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
