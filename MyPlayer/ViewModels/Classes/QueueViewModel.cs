﻿using MyPlayer.CommonClasses;
using MyPlayer.Models;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{

    [JsonObject(MemberSerialization.OptIn)]
    public class QueueViewModel : BaseModel, IQueueViewModel
    {
        private readonly IQueue _queue;

        public IList<VisualObject<IMediaBase>> Artists { get; private set; } = null;

        private void UpdateArtists()
        {
            var inner = _queue.Artists.Select(a => new VisualObject<IMediaBase>(a, this));
            //if (!string.IsNullOrWhiteSpace(SearchText))
            {
                //inner = inner.Where(a => a.Children.Where(s => s.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)));
            }
            Artists = inner.ToList();
            RaisePropertyChanged(nameof(Artists));
        }

        public LoopType LoopType
        {
            get => _queue.LoopType;
            set
            {
                _queue.LoopType = value;
                RaisePropertyChanged();
            }
        }


        public void UpdateSongs()
        {
            _songs = null;
            _ = Songs?.Count;
        }

        private IList<ISong> _songs;

        private IList<ISong> Songs
        {
            get
            {
                if (_songs == null && Artists != null)
                {
                    Func<VisualObject<IMediaBase>, bool> predicate;
                    //if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        predicate = s => !s.Data.HasChildren && s.Data.IsSelected;
                    }
                    //else
                    //{
                    //    predicate = s => s.Data.GetType() == typeof(Song) && s.IsSelected && s.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
                    //}
                    _songs = Artists.Flatten(a => a.Children).Where(predicate).Select(a => (ISong)a.Data).ToList();
                }
                return _songs;
            }
        }


        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SearchIsEmpty = string.IsNullOrWhiteSpace(value);
                Set(ref _searchText, value);
            }
        }

        public bool SearchIsEmpty { get; private set; }

        private bool _showAlbums;
        [JsonProperty]
        public bool ShowAlbums
        {
            get => _showAlbums;
            set
            {
                if (Artists != null)
                {
                    foreach (var artist in Artists)
                    {
                        artist.IsExpanded = value;
                    }
                }
                Set(ref _showAlbums, value);
                RaisePropertyChanged(nameof(AlbumsToolItemText));
            }
        }

        private bool _showSongs;
        [JsonProperty]
        public bool ShowSongs
        {
            get => _showSongs;
            set
            {
                if (Artists != null)
                {
                    foreach (var artist in Artists)
                    {
                        foreach (var album in artist.Children)
                        {
                            album.IsExpanded = value;
                        }
                    }
                }
                Set(ref _showSongs, value);
                RaisePropertyChanged(nameof(SongsToolItemText));
            }
        }

        public string AlbumsToolItemText => ShowAlbums ? "Hide albums" : "Show albums";
        public string SongsToolItemText => ShowSongs ? "Hide songs" : "Show songs";

        private bool _allSelected = true;

        public bool AllSelected
        {
            get => _allSelected;
            set
            {
                Set(ref _allSelected, value);
                RaisePropertyChanged(nameof(AllSelectedImageSource));
            }
        }
        public string AllSelectedImageSource => "baseline_check_box_black_36dp.png";
        //AllSelected ? "baseline_check_box_black_36dp.png" : "baseline_check_box_outline_blank_black_36dp.png";

        public ICommand ClearSearchTextCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }

        public ICommand ShowAlbumsCommand { get; set; }
        public ICommand ShowSongsCommand { get; set; }
        public ICommand PlayTappedCommand { get; set; }
        public ICommand PlayFirstChildCommand { get; set; }


        public QueueViewModel(IQueue queue)
        {
            CreateCommands();
            _queue = queue;
            SearchText = "";
            ShowAlbums = true;
            ShowSongs = false;
        }

        #region Commands
        private void CreateCommands()
        {
            ClearSearchTextCommand = new Command(ClearSearchTextAction);

            ShowAlbumsCommand = new Command(ShowAlbumsAction);
            ShowSongsCommand = new Command(ShowSongsAction);

            SelectAllCommand = new Command(SelectAllAction);
            PlayTappedCommand = new Command(PlayTappedAction);
            PlayFirstChildCommand = new Command(PlayFirstChildAction);
        }

        private async void PlayFirstChildAction(object obj)
        {
            if (!(obj is VisualObject<IMediaBase> selected))
            {
                return;
            }
            ISong song = null;
            if (selected.Data is IAlbum album)
            {
                song = (ISong)album.Children.FirstOrDefault();
            }
            else if (selected.Data is IArtist artist)
            {
                song = (ISong)artist.Children.FirstOrDefault()?.Children.FirstOrDefault();
            }
            MessagingCenter.Send<BaseModel, ISong>(this, "SongSelected", song);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void PlayTappedAction(object obj)
        {
            if (!(obj is VisualObject<IMediaBase> selected))
            {
                return;
            }
            MessagingCenter.Send<BaseModel, ISong>(this, "SongSelected", (ISong)selected.Data);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void ShowSongsAction(object obj)
        {
            ShowSongs = !ShowSongs;
        }

        private void ShowAlbumsAction(object obj)
        {
            ShowAlbums = !ShowAlbums;
        }




        private void ClearSearchTextAction(object obj)
        {
            SearchText = "";
        }

        private void SelectAllAction(object obj)
        {
            AllSelected = !AllSelected;
        }
        #endregion

        public void AddFromRoot(string path)
        {
            _queue.Clear();
            _queue.AddFromRoot(path);
            UpdateArtists();
            UpdateSongs();
        }

      
    }


}
