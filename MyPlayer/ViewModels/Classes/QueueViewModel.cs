using MyPlayer.CommonClasses;
using MyPlayer.Models;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyPlayer.ViewModels
{

    [JsonObject(MemberSerialization.OptIn)]
    public class QueueViewModel : BaseModel, IQueueViewModel
    {
        private IQueue _queue;

        public IReadOnlyCollection<VisualObject<IMediaBase>> Artists { get; private set; } = null;

        private void RetreiveArtists()
        {
                Artists = new ReadOnlyCollection<VisualObject<IMediaBase>>(_queue.Artists.Select(a => new VisualObject<IMediaBase>(a, this)).ToList());
        }

        public void UpdateSongs()
        {
            _songs = null;
            _ = Songs.Count;
        }

        private IList<ISong> _songs;

        private IList<ISong> Songs
        {
            get
            {
                if (_songs == null)
                {
                    _songs = Artists.Flatten(a => a.Children).Where(s => s.Data.GetType() == typeof(Song) && s.IsSelected).Select(a => (ISong)a.Data).ToList();
                }
                return _songs;
            }
        }
    

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

        //private int _index = 0;

        //public int Index
        //{
        //    get
        //    {
        //        _index++;
        //        return _index;
        //    }
        //}

        private bool _showAlbums;
        [JsonProperty]
        public bool ShowAlbums
        {
            get => _showAlbums;
            set
            {
                Set(ref _showAlbums, value);
                RaisePropertyChanged("AlbumsToolItemText");
            }
        }

        private bool _showSongs;
        [JsonProperty]
        public bool ShowSongs
        {
            get => _showSongs;
            set
            {
                Set(ref _showSongs, value);
                RaisePropertyChanged("TotalHeight");
                RaisePropertyChanged("SongsToolItemText");
                //foreach (var artist in Artists)
                //{
                //    artist.DoPropertyChanged("Height");
                //}
            }
        }

        public string AlbumsToolItemText => ShowAlbums ? "Hide albums" : "Show albums";
        public string SongsToolItemText => ShowSongs ? "Hide songs" : "Show songs";

        public int TotalHeight => Artists == null ? 500 : Artists.Sum(artist => artist.Height);

        private bool _allSelected = true;

        public bool AllSelected
        {
            get => _allSelected;
            set
            {
                Set(ref _allSelected, value);
                RaisePropertyChanged("AllSelectedImageSource");
            }
        }
        public string AllSelectedImageSource => AllSelected ? "baseline_check_box_black_36dp.png" : "baseline_check_box_outline_blank_black_36dp.png";

        public ICommand ClearSearchTextCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }

        public ICommand ShowAlbumsCommand { get; set; }
        public ICommand ShowSongsCommand { get; set; }
        public ICommand PlayTappedCommand { get; set; }

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
        public ISong Current { get; set; }
        //public int Id { get; set; }

        public QueueViewModel()
        {
            CreateCommands();
            _queue = new Queue();
            SearchText = "";
            ShowAlbums = true;
            ShowSongs = true;
        }

        #region Commands
        private void CreateCommands()
        {
            ClearSearchTextCommand = new Command(ClearSearchTextAction/*, CanClearSearchText*/);

            ShowAlbumsCommand = new Command(ShowAlbumsAction);
            ShowSongsCommand = new Command(ShowSongsAction);

            SelectAllCommand = new Command(SelectAllAction);
            PlayTappedCommand = new Command(PlayTappedAction);
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



        private bool CanClearSearchText(object arg)
        {
            return !string.IsNullOrWhiteSpace(SearchText);
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
            RetreiveArtists();
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
            var newSong = Songs.Next(song);
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
            var newSong = Songs.Previous(song);
            if (newSong == null && LoopType == LoopType.All)
            {
                newSong = Songs.LastOrDefault();
            }
            return newSong;
        }

        public ISong GetDefault()
        {
            if (Songs != null)
            {
                return Songs.FirstOrDefault();
            }
            return Artists.FirstOrDefault().Children?.FirstOrDefault()?.Children?.FirstOrDefault()?.Data as ISong;
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
