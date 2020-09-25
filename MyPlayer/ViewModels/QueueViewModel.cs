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

        //public List<ArtistModel> Songs => Queue.Songs
        //    .GroupBy(song => song.Album).Select(a => new AlbumModel { Album = a.Key, Songs = a.ToList() }).ToList()
        //    .GroupBy(album => album.Album.Artist).Select(a => new ArtistModel { Artist = a.Key, Albums = a.Key.Albums.ToList() }).ToList()
        //    ;
        public List<IGrouping<IArtist, AlbumModel>> Songs => Queue.Songs
                .GroupBy(song => song.Album).Select(a => new AlbumModel { Album = a.Key, Songs = a.ToList() }).ToList()
                .GroupBy(album => album.Album.Artist).ToList();


        public ICommand BrowseFilesCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => Set(nameof(SearchText), ref _searchText, value);
        }

        public QueueViewModel(IQueue queue)
        {
            CreateCommands();
            Queue = queue;
            SearchText = "";
        }

        #region Commands
        private void CreateCommands()
        {
            BrowseFilesCommand = new Command(BrowseFilesAction, CanBrowseFiles);
            ClearSearchTextCommand = new Command(ClearSearchTextAction/*, CanClearSearchText*/);
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
        #endregion



      

    }
}
