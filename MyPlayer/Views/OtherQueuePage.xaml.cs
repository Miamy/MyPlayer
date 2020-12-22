using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtherQueuePage : ContentPage
    {
        private readonly IQueueViewModel _model;
        public OtherQueuePage(IQueueViewModel model)
        {
            InitializeComponent();

            BindingContext = _model = model;
            _model.PropertyChanged += ModelPropertyChanged;
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "Height" || e.PropertyName == "ShowSongs")
            {
                //ArtistsView.Layout(ArtistsView.Bounds);
                /*ArtistsView.BatchBegin();
                BindingContext = null;
                BindingContext = _model;
                ArtistsView.BatchCommit();*/
            }
        }

        protected override async void OnDisappearing()
        {
            await Task.Run(() => Storage.SaveQueue(_model));
            _model.UpdateSongs();
            base.OnDisappearing();
        }
    }
}