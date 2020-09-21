using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Song : MediaBase, ISong
    {

        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public IFile Container { get; set; }

        private IAlbum _album;
        public IAlbum Album 
        { 
            get => _album;
            set
            {
                _album = value;
                if (_album != null)
                {
                    _album.AddSong(this);
                }
            }
        }

        public TimeSpan OffcetInContainer { get; set; }

        public Song(IMusicFile file) : this(file, Consts.ZeroTimeSpan)
        {
        }

        public Song(IMusicFile file, TimeSpan offcet)
        {
            Container = file;
            Name = Path.GetFileNameWithoutExtension(file.Name);
            Duration = Consts.ZeroTimeSpan;
            OffcetInContainer = offcet;

            MediaInfo.Setup(file.FullPath);
        }

        public override string ToString()
        {
            var result = Name;
            if (Album != null)
            {
                result = Album.ToString() + " / " + result;
            }
            return result;
        }
    }
}
