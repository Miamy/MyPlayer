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

namespace MyPlayer.ViewModels
{

    public class QueueViewModel : BaseViewModel
    {

        public IQueue Queue { get; set; }

        //public List<ArtistModel> Artists => Queue.Songs
        //        .GroupBy(song => song.Album).Select(a => new AlbumModel(a.Key))
        //        .GroupBy(album => album.Artist).Select(a => new ArtistModel(a.Key)).ToList()
        public IList<ArtistModel> Artists => Queue.Artists.Select(a => new ArtistModel(a, this)).ToList();


        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

        private int _index = 0;

        public int Index
        {
            get
            {
                _index++;
                return _index;
            }
        }

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
            }
        }

        public string AlbumsToolItemText => ShowAlbums ? "Hide albums" : "Show albums";
        public string SongsToolItemText => ShowSongs ? "Hide songs" : "Show songs";

        public int TotalHeight => Artists.Sum(artist => artist.DiscographyTotalHeight);

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

        public ICommand BrowseFilesCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }

        public ICommand ShowAlbumsCommand { get; set; }
        public ICommand ShowSongsCommand { get; set; }

        public QueueViewModel(IQueue queue)
        {
            CreateCommands();
            Queue = queue;
            SearchText = "";
            ShowAlbums = true;
            ShowSongs = true;
        }

        #region Commands
        private void CreateCommands()
        {
            BrowseFilesCommand = new Command(BrowseFilesAction, CanBrowseFiles);
            ClearSearchTextCommand = new Command(ClearSearchTextAction/*, CanClearSearchText*/);

            ShowAlbumsCommand = new Command(ShowAlbumsAction);
            ShowSongsCommand = new Command(ShowSongsAction);

            SelectAllCommand = new Command(SelectAllAction);
        }

        private void ShowSongsAction(object obj)
        {
            ShowSongs = !ShowSongs;
        }

        private void ShowAlbumsAction(object obj)
        {
            ShowAlbums = !ShowAlbums;
        }

        private bool CanBrowseFiles(object arg)
        {
            return true;
        }

        private async void BrowseFilesAction(object obj)
        {
            var page = new BrowsePage();
            await Application.Current.MainPage.Navigation.PushAsync(page, false);
        }

        private bool CanClearSearchText(object arg)
        {
            return SearchText != "";
        }

        private void ClearSearchTextAction(object obj)
        {
            SearchText = "";
        }

        private void SelectAllAction(object obj)
        {
            AllSelected = !AllSelected;
            foreach (var artist in Artists)
            {
                artist.IsSelected = AllSelected;
            }
        }
        #endregion





    }
}
