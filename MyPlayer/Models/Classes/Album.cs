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
                _artist.AddAlbum(this);
            }
        }

        public int Year { get; set; }
        public List<string> Covers { get; set; }

        public Album(IArtist artist, string name, string container) : base(name, container)
        {
            Artist = artist ?? throw new ArgumentNullException("artist");
            Children = new List<IMediaBase>();
            Covers = new List<string>();
            FillCovers(container);
        }

        private void FillCovers(string container)
        {
            var files = PathScanner.GetCovers(container);
            if (files.Length > 0)
            {
                Covers.Add(files[0]);
            }

            var scansFolder = Path.Combine(container, "scans");
            if (Directory.Exists(scansFolder))
            {
                files = PathScanner.GetCovers(scansFolder);
                Covers.AddRange(files);
            }
        }

        public void AddSong(ISong song)
        {
            if (Children.IndexOf(song) != -1)
            {
                return;
            }
            Children.Add(song);
            //_duration += song.Duration;
        }

        public override string ToString()
        {
            return Artist.ToString() + " / " + Name;
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
