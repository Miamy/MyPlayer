using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System.ComponentModel;
using MyPlayer.Models.Classes;

namespace MyPlayer.Views
{
    public partial class QueuePage : ContentPage
    {
        private readonly IQueueViewModel _model;

        public QueuePage(IQueueViewModel model)
        {
            InitializeComponent();
            _model = model;
            BindingContext = _model;

        }

        protected override async void OnDisappearing()
        {
            await Task.Run(() => Storage.SaveQueue(_model));
            base.OnDisappearing();
        }
    }
}