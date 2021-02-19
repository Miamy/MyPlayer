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
                _album.AddSong(this);
            }
        }

        public TimeSpan OffcetInContainer { get; set; }

        private TimeSpan _duration = TimeSpan.Zero;
        public override TimeSpan Duration
        {
            get => _duration;
            set => _duration = value;
        }


        public Song(IAlbum album, string name, string container) : base(name, container)
        {
            Album = album ?? throw new ArgumentNullException("album");

            var tagFile = TagLib.File.Create(new FileAbstraction(container));
            Duration = tagFile.Properties.Duration;
        }

        public override string ToString()
        {
            return Album.ToString() + " / " + Name;
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
