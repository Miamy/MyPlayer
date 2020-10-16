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
        public TimeSpan Duration { get; set; }

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


        public Song(string name, string container) : this(name, container, Consts.ZeroTimeSpan)
        {
        }

        public Song(string name, string container, TimeSpan offcet) : base(name, container)
        {
            Duration = Consts.ZeroTimeSpan;
            OffcetInContainer = offcet;
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

        public override string GetUniqueName()
        {
            return Container + ":::" + OffcetInContainer.ToString();
        }
    }
}
