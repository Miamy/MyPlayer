using MyPlayer.CommonClasses;
using MyPlayer.Models;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.Views;
using System;
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

    public class QueueViewModel : BaseViewModel, IQueueViewModel
    {

        private IQueue _queue;

        public IReadOnlyCollection<VisualObject<IArtist>> Artists { get; private set; } = null;
        //{ get; private set; }

        private void RetreiveArtists(bool force)
        {
            if (Artists == null || force)
            {
                Artists = new ReadOnlyCollection<VisualObject<IArtist>>(_queue.Artists.Select(a => new VisualObject<IArtist>(a, this)).ToList());
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

        public int TotalHeight => Artists.Sum(artist => artist.Height);

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
        public LoopType LoopType
        {
            get => _loopType;
            set
            {
                _loopType = value;
                RaisePropertyChanged();
            }
        }
        public ISong Current { get; set; }
        public int Id { get; set; }

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
            MessagingCenter.Send<BaseViewModel, IMediaBase>(this, "SongSelected", selected.Data);
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
            _queue.AddFromRoot(path);
            RetreiveArtists(true);
        }

        public ISong Next(ISong song)
        {
            if (LoopType == LoopType.One)
            {
                return song;
            }

            var newSong = _queue.Songs.Next(song);
            if (newSong == null && LoopType == LoopType.All)
            {
                newSong = _queue.Songs.FirstOrDefault();
            }
            return newSong;
        }
        public ISong Prev(ISong song)
        {
            if (LoopType == LoopType.One)
            {
                return song;
            }

            var newSong = _queue.Songs.Previous(song);
            if (newSong == null && LoopType == LoopType.All)
            {
                newSong = _queue.Songs.LastOrDefault();
            }
            return newSong;
        }

        public ISong GetDefault()
        {
            return _queue.Songs.FirstOrDefault();
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

    public static class ExtensionMethods
    {
        public static T Previous<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item) - 1;
            return index > -1 ? list[index] : default;
        }
        public static T Next<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item) + 1;
            return index < list.Count() ? list[index] : default;
        }
        public static T Previous<T>(this IList<T> list, Func<T, Boolean> lookup)
        {
            var item = list.SingleOrDefault(lookup);
            var index = list.IndexOf(item) - 1;
            return index > -1 ? list[index] : default;
        }
        public static T Next<T>(this IList<T> list, Func<T, Boolean> lookup)
        {
            var item = list.SingleOrDefault(lookup);
            var index = list.IndexOf(item) + 1;
            return index < list.Count() ? list[index] : default;
        }
        public static T PreviousOrFirst<T>(this IList<T> list, T item)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");

            var previous = list.Previous(item);
            return previous == null ? list.First() : previous;
        }
        public static T NextOrLast<T>(this IList<T> list, T item)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var next = list.Next(item);
            return next == null ? list.Last() : next;
        }
        public static T PreviousOrFirst<T>(this IList<T> list, Func<T, Boolean> lookup)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var previous = list.Previous(lookup);
            return previous == null ? list.First() : previous;
        }
        public static T NextOrLast<T>(this IList<T> list, Func<T, Boolean> lookup)
        {
            if (list.Count() < 1)
                throw new Exception("No array items!");
            var next = list.Next(lookup);
            return next == null ? list.Last() : next;
        }
    }
}
