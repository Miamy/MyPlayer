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
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = Path.GetFileNameWithoutExtension(value);
            }
        }


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

        private TimeSpan _duration = TimeSpan.Zero;
        public override TimeSpan Duration
        {
            get => _duration;
            set => _duration = value;
        }


        public Song(string name, string container) : this(name, container, Consts.ZeroTimeSpan)
        {
        }

        public Song(string name, string container, TimeSpan offcet) : base(name, container)
        {
            OffcetInContainer = offcet;

            var tagFile = TagLib.File.Create(new FileAbstraction(container));
            if (!tagFile.Properties.Description.Contains("Flac"))
            {
                Duration = tagFile.Properties.Duration;
            }
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

    public class FileAbstraction : TagLib.File.IFileAbstraction
    {
        public FileAbstraction(string file)
        {
            Name = file;
        }

        public string Name { get; }

        public Stream ReadStream => new FileStream(Name, FileMode.Open, FileAccess.Read);

        public Stream WriteStream => new FileStream(Name, FileMode.Open, FileAccess.Read);

        public void CloseStream(Stream stream)
        {
            stream.Close();
        }
    }
}
