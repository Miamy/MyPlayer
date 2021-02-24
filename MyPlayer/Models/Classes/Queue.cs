using CueSharp;
using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Queue : BaseModel, IQueue
    {
        private IList<ISong> _songs;
        public IList<ISong> Songs => _songs;
        public IList<IMediaBase> Artists { get; set; }

        private LoopType _loopType = LoopType.All;

        [JsonProperty]
        public LoopType LoopType
        {
            get => _loopType;
            set
            {
                _loopType = value;
                RaisePropertyChanged();
            }
        }

        [JsonProperty]
        private ISong _current;
        public ISong Current 
        { 
            get => _current;
            set
            {
                Set(ref _current, value);
            }
        }

        public Queue()
        {
            Artists = new List<IMediaBase>();
        }

        private void UpdateSongs()
        {
            Func<IMediaBase, bool> predicate = s => !s.HasChildren && s.IsSelected;
            _songs = Artists.Flatten(a => a.Children).Where(predicate).Select(a => (ISong)a).ToList();
        }

        public void Fill(string path)
        {
            Clear();
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                return;
            }

            FillArtists(path);
            Current = Songs.FirstOrDefault();
        }

        private static string GetName(string path)
        {
            return path[(path.LastIndexOf("/") + 1)..];
        }

        private void FillArtists(string path)
        {
            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var folder in folders)
            {
                var name = GetName(folder);
                var artist = new Artist(name, folder);
                Artists.Add(artist);

                FillAlbums(artist);
            }

            UpdateSongs();
        }


        private void FillAlbums(IArtist artist)
        {
            var folders = Directory.GetDirectories(artist.Container, "*", SearchOption.TopDirectoryOnly);
            foreach (var folder in folders)
            {
                var name = GetName(folder);
                var album = new Album(artist, name, folder);

                FillSongs(album);
            }
        }

        private void FillSongs(IAlbum album)
        {
            FillMp3Songs(album);
            FillFlacSongs(album);
        }

        private void FillMp3Songs(IAlbum album)
        {
            var files = Directory.GetFiles(album.Container, "*.mp3", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var name = GetName(file);
                _ = new Song(album, name, file);
            }
        }
        private void FillFlacSongs(IAlbum album)
        {
            var files = Directory.GetFiles(album.Container, "*.flac", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var descriptionFilename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".cue");
                if (File.Exists(descriptionFilename))
                {
                    var tagFile = TagLib.File.Create(new FileAbstraction(file));
                    var totalDuration = tagFile.Properties.Duration;

                    var cueSheet = new CueSheet(descriptionFilename);
                    for (var i = 0; i < cueSheet.Tracks.Length; i++)
                    {
                        var track = cueSheet.Tracks[i];
                        var song = new Song(album, track.Title, file)
                        {
                            Duration = TimeSpan.Zero
                        };
                        if (track.Indices.Length > 0)
                        {
                            var index = track.Indices[0];
                            song.OffcetInContainer = new TimeSpan(0, 0, index.Minutes, index.Seconds, index.Frames / 75 * 1000);
                            if (i == cueSheet.Tracks.Length - 1)
                            {
                                song.Duration = totalDuration - song.OffcetInContainer;
                            }
                            else
                            {
                                var nextTrack = cueSheet.Tracks[i + 1];
                                if (nextTrack.Indices.Length > 0)
                                {
                                    index = nextTrack.Indices[0];
                                    var nextOffcet = new TimeSpan(0, 0, index.Minutes, index.Seconds, index.Frames / 75 * 1000);
                                    song.Duration = nextOffcet - song.OffcetInContainer;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var name = GetName(file);
                    _ = new Song(album, name, file);
                }
            }
        }


        public void Clear()
        {
            Artists.Clear();
            UpdateSongs();
        }

        public ISong Next(ISong song)
        {
            if (LoopType == LoopType.One)
            {
                return song;
            }
            if (song == null)
            {
                return null;
            }
            var newSong = Songs.Where(song => song.IsSelected).ToList().Next(song);
            if (newSong == null && LoopType == LoopType.All)
            {
                newSong = Songs.FirstOrDefault();
            }
            return newSong;
        }
        public ISong Prev(ISong song)
        {
            if (LoopType == LoopType.One)
            {
                return song;
            }
            if (song == null)
            {
                return null;
            }
            var newSong = Songs.Where(song => song.IsSelected).ToList().Previous(song);
            if (newSong == null && LoopType == LoopType.All)
            {
                newSong = Songs.LastOrDefault();
            }
            return newSong;
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
