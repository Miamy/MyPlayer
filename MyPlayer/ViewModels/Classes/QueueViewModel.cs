using MyPlayer.CommonClasses;
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
        private IQueue _queue;
        public IQueue Queue
        {
            get => _queue;
            set
            {
                _queue = value;
                UpdateArtists();
            }
        }

        public IList<VisualObject<IMediaBase>> Artists { get; private set; } = null;

        private void UpdateArtists()
        {
            if (Queue == null)
            {
                return;
            }
            var inner = Queue.Artists.Select(a => new VisualObject<IMediaBase>(a, this));
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

        private bool _expandAlbums;
        [JsonProperty]
        public bool ExpandAlbums
        {
            get => _expandAlbums;
            set
            {
                if (Artists != null)
                {
                    foreach (var artist in Artists)
                    {
                        artist.IsExpanded = value;
                    }
                }
                Set(ref _expandAlbums, value);
                RaisePropertyChanged(nameof(AlbumsToolItemText));
            }
        }

        private bool _expandSongs;
        [JsonProperty]
        public bool ExpandSongs
        {
            get => _expandSongs;
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
                Set(ref _expandSongs, value);
                RaisePropertyChanged(nameof(SongsToolItemText));
            }
        }

        public string AlbumsToolItemText => ExpandAlbums ? "Collapse albums" : "Expand albums";
        public string SongsToolItemText => ExpandSongs ? "Collapse songs" : "Expand songs";

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

        public ICommand ExpandAlbumsCommand { get; set; }
        public ICommand ExpandSongsCommand { get; set; }
        public ICommand PlayTappedCommand { get; set; }


        public QueueViewModel(IQueue queue)
        {
            CreateCommands();
            Queue = queue ?? throw new ArgumentNullException("queue"); 
            SearchText = "";
            ExpandAlbums = true;
            ExpandSongs = false;
        }

        #region Commands
        private void CreateCommands()
        {
            ClearSearchTextCommand = new Command(ClearSearchTextAction);

            ExpandAlbumsCommand = new Command(ExpandAlbumsAction);
            ExpandSongsCommand = new Command(ExpandSongsAction);

            SelectAllCommand = new Command(SelectAllAction);
            PlayTappedCommand = new Command(PlayTappedAction);
        }

   
        private async void PlayTappedAction(object obj)
        {
            if (!(obj is VisualObject<IMediaBase> data))
            {
                return;
            }

            ISong newSong = null;
            if (data.Data is Song song)
            {
                newSong = song;
            }
            else if (data.Data is Album album)
            {
                newSong = (ISong)album.Children.FirstOrDefault();
            }
            else if (data.Data is Artist artist)
            {
                newSong = (ISong)((IAlbum)artist.Children.FirstOrDefault()).Children.FirstOrDefault();
            }

            MessagingCenter.Send<BaseModel, ISong>(this, "SongSelected", newSong);
            await App.Navigation.PopAsync();
        }

        private void ExpandSongsAction(object obj)
        {
            ExpandSongs = !ExpandSongs;
        }

        private void ExpandAlbumsAction(object obj)
        {
            ExpandAlbums = !ExpandAlbums;
        }




        private void ClearSearchTextAction(object obj)
        {
            SearchText = "";
        }

        private void SelectAllAction(object obj)
        {
            AllSelected = !AllSelected;
        }

        public bool SearchTextPresent(string name)
        {
            return SearchIsEmpty || name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion



    }


}
