using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class QueueViewModel : INotifyPropertyChanged
    {
        public IQueue Queue { get; set; }

        //public List<SongModel> Songs => Queue.Songs.Select(song => new SongModel(Queue.Songs.IndexOf(song) + 1, song)).ToList();
        //public List<ISong> Songs => Queue.Songs.ToList();

        //public List<SongModel> Songs => Queue.Songs.GroupBy(song => song.Album.Artist).GroupBy(song => song.Key.Albums).Select(song => new SongModel(Queue.Songs.IndexOf(song.) + 1, song)).ToList();
        public List<IGrouping<IArtist, IGrouping<IAlbum, ISong>>> Songs => Queue.Songs.GroupBy(song => song.Album).GroupBy(song => song.Key.Artist).ToList();

        //public List<AlbumModel> Albums => Queue.Albums.Select(album => new AlbumModel(album.Name, album.Songs)).ToList();

        //public List<IArtist> Artists => Queue.Artists.ToList();
        //public List<ArtistModel> Artists => Queue.Artists.Select(artist => new ArtistModel(artist.Name, artist.Albums)).ToList();


        public ICommand AddToQueueCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }

        private string _searchText;
        public string SearchText 
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged("SearchText");
            }
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
            AddToQueueCommand = new Command(AddToQueueAction, CanAddToQueue);
            ClearSearchTextCommand = new Command(ClearSearchTextAction/*, CanClearSearchText*/);
        }


        private bool CanAddToQueue(object arg)
        {
            return true;
        }

        private void AddToQueueAction(object obj)
        {
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



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string aName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aName));
        }

    }
}
