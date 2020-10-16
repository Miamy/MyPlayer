using MyPlayer.CommonClasses;
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
    public class Queue : IQueue, INotifyPropertyChanged 
    {
        public IList<ISong> Songs { get; set; }
        public IList<IAlbum> Albums { get; set; }
        public IList<IArtist> Artists { get; set; }

        public IList<IPathElement> PathElements { get; set; }


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
            PathElements = new List<IPathElement>();
        }

        public void AddFromRoot(string path)
        {
            PathElements.Clear();
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Bad path {path}");
            }

            var pathElement = new PathElement("", path);
            PathElements.Add(pathElement);
            ProceedPath(pathElement, path);
        }

        private void ProceedPath(IPathElement parent, string path)
        {
            IPathElement pathElement;

            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                pathElement = new PathElement(parent, folder);
                PathElements.Add(pathElement);

                ProceedPath(pathElement, folder);
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (PathElement.IsMusicFile(file))
                {
                    pathElement = new PathElement(parent, file);
                    Add(pathElement);
                }
                else
                {
                    pathElement = new PathElement(parent, file);
                }
                PathElements.Add(pathElement);
            }
        }

        private static IAlbum lastAlbum = null;
        private static IArtist lastArtist = null;

        private void AddSong(ISong song)
        {
            Songs.Add(song);

            var songPath = song.Container;
            var albumName = PathElement.GetParent(songPath);
       

            if (!string.IsNullOrWhiteSpace(albumName))
            {
                var album = lastAlbum;
                if (album?.Name != albumName)
                {
                    album = Albums.FirstOrDefault(a => a.Name == albumName);
                }
                if (album == null)
                {
                    album = new Album(albumName, songPath.GetOneLevelUp());
                    Albums.Add(album);
                }
                song.Album = album;
                lastAlbum = album;
            }

            var artistName = PathElement.GetGrandParent(songPath);
            if (!string.IsNullOrWhiteSpace(artistName))
            {
                var artist = lastArtist;
                if (artist?.Name != artistName)
                {
                    artist = Artists.FirstOrDefault(a => a.Name == artistName);
                }
                
                if (artist == null)
                {
                    artist = new Artist(artistName, songPath.GetLevelUp(2));
                    Artists.Add(artist);
                }
                if (song.Album != null)
                {
                    song.Album.Artist = artist;
                }
                lastArtist = artist;
            }
        }

        public void Add(IPathElement item)
        {
            if (item.IsComposite && item.HasDescription)
            {
                AddSeveralSongs(item);
            }
            else
            {
                var song = new Song(item.Name, item.FullPath);
                AddSong(song);
            }
        }

        private void AddSeveralSongs(IPathElement item)
        {
            ISong song = null;
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
                    song = new Song(item.Name, item.FullPath);
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

        public void Remove(IPathElement item)
        {

        }    

       
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }   
    }
}
