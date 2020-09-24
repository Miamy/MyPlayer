using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Queue : IQueue
    {
        public IList<ISong> Songs { get; set; }
        public IList<IAlbum> Albums { get; set; }
        public IList<IArtist> Artists { get; set; }


        private LoopType _loopType = LoopType.All;
        public LoopType LoopType
        {
            get => _loopType;  
            set
            {
                _loopType = value;
                RaisePropertyChanged();
            }
        }

        public Queue()
        {
            Songs = new List<ISong>();
            Albums = new List<IAlbum>();
            Artists = new List<IArtist>();
        }

        private void AddSong(ISong song)
        {
            Songs.Add(song);

            var albumName = song.MediaInfo.AlbumName;
            if (!string.IsNullOrWhiteSpace(albumName))
            {
                var album = Albums.FirstOrDefault(a => a.Name == albumName);
                if (album == null)
                {
                    album = new Album(albumName);
                    album.Year = album.MediaInfo.Year = song.MediaInfo.Year;
                    album.MediaInfo.Cover = song.MediaInfo.Cover;
                    Albums.Add(album);
                }
                song.Album = album;
            }

            var artistName = song.MediaInfo.Artist;
            if (!string.IsNullOrWhiteSpace(artistName))
            {
                var artist = Artists.FirstOrDefault(a => a.Name == artistName);
                if (artist == null)
                {
                    artist = new Artist(artistName);
                    Artists.Add(artist);
                }
                if (song.Album != null)
                {
                    song.Album.Artist = artist;
                }
            }

            //RaisePropertyChanged("Songs");
        }

        public void Add(IMusicFile item)
        {
            ISong song = null;
            if (item.IsComposite && item.HasDescription)
            {
                var content = File.ReadAllLines(item.DescriptionFilename);
                for (var i = 0; i < content.Length; i++)
                {
                    var line = content[i].Trim();

                    if (line.StartsWith("TRACK"))
                    {
                        if (song != null)
                        {
                            AddSong(song);
                        }
                        song = new Song(item);
                    }

                    if (line.StartsWith("TITLE"))
                    {
                        if (song != null)
                        {
                            song.Name = line.Replace("TITLE", "").Replace("\"", "").Trim();
                        }
                    }

                    if (line.StartsWith("INDEX 01"))
                    {
                        var offcet = line.Replace("INDEX 01", "").Trim();
                        var parts = offcet.Split(':');
                        if (parts.Length == 3)
                        {
                            try
                            {
                                song.OffcetInContainer = new TimeSpan(0, 0, int.Parse(parts[0]), int.Parse(parts[1]), (int)(float.Parse(parts[2]) / 75 * 1000));
                            }
                            catch
                            {
                                song.OffcetInContainer = Consts.ZeroTimeSpan;
                            }
                        }
                    }
                }
                if (song != null)
                {
                    AddSong(song);
                }

                for (var i = 0; i < Songs.Count - 2; i++)
                {
                    if (Songs[i + 1].OffcetInContainer > Consts.ZeroTimeSpan)
                    {
                        Songs[i].Duration = Songs[i + 1].OffcetInContainer - Songs[i].OffcetInContainer;
                    }
                }
            }
            else
            {
                song = new Song(item);
                AddSong(song);
            }
        }

        public void AddRange(IEnumerable<IMusicFile> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void AddRange2(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                var file = new MusicFile(item);
                Add(file);
            }
        }

        public void Remove(IPathElement item)
        {

        }

        public void Play()
        {
        }

        public void Pause()
        {
        }

        public void Stop()
        {
        }

        public ISong Next(ISong song)
        {
            var index = Songs.IndexOf(song);
            if (index == -1)
            {
                return GetDefault();
            }

            switch (LoopType)
            {
                case LoopType.None:
                    if (index == Songs.Count - 1)
                    {
                        return Songs[Songs.Count - 1];
                    }
                    break;
                case LoopType.One:
                    return song;
                case LoopType.All:
                    if (index == Songs.Count - 1)
                    {
                        return Songs[0];
                    }
                    break;
            }
            return Songs[index + 1];
        }

        public ISong Prev(ISong song)
        {
            var index = Songs.IndexOf(song);
            if (index == -1)
            {
                return GetDefault();
            }

            switch (LoopType)
            {
                case LoopType.None:
                    if (index == 0)
                    {
                        return Songs[0];
                    }
                    break;
                case LoopType.One:
                    return song;
                case LoopType.All:
                    if (index == 0)
                    {
                        return Songs[Songs.Count - 1];
                    }
                    break;
            }
            return Songs[index - 1];
        }

        public ISong GetDefault()
        {
            return Songs.FirstOrDefault();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string aName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aName));
        }

        public void SwitchLoopType()
        {
            switch (LoopType)
            {
                case LoopType.None:
                    LoopType = LoopType.One;
                    break;
                case LoopType.One:
                    LoopType = LoopType.All;
                    break;
                case LoopType.All:
                    LoopType = LoopType.None;
                    break;
            }
        }
    }
}
