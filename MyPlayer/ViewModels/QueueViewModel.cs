using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class AlbumModel
    {
        public IAlbum Key { get; set; }
        public int Count { get; set; }
        public List<ISong> Songs { get; set; }
    }

    public class ArtistModel
    {
        public IArtist Key { get; set; }
        public int Count { get; set; }
        //public List<IGrouping<IAlbum, ISong>> Songs { get; set; }
        public List<IAlbum> Albums { get; set; }
    }

    public class QueueViewModel : INotifyPropertyChanged
    {

        public IQueue Queue { get; set; }

        //public List<ArtistModel> Songs => Queue.Songs
        //    .GroupBy(song => song.Album).Select(a => new AlbumModel { Key = a.Key, Count = a.Count(), Songs = a.ToList() }).ToList()
        //    .GroupBy(album => album.Key.Artist).Select(a => new ArtistModel { Key = a.Key, Count = a.Count(), Albums = a.Key.Albums.ToList() }).ToList()
        //    ;
        public List<IGrouping<IArtist, IGrouping<IAlbum, ISong>>> Songs => Queue.Songs
                .GroupBy(song => song.Album)
                .GroupBy(album => album.Key.Artist).ToList();

        //public int BlockHeight { get; set; } = 300;

        public ICommand AddToQueueCommand { get; set; }
        public ICommand ClearSearchTextCommand { get; set; }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged();
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
        protected virtual void RaisePropertyChanged([CallerMemberName] string aName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aName));
        }

    }
}
