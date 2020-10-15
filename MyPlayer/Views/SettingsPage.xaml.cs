using MyPlayer.CommonClasses;
using MyPlayer.Models.Classes;
using MyPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel _model;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _model = new SettingsViewModel(Settings.Instance);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Storage.LoadSettings(_model.Settings);
        }

        protected override void OnDisappearing()
        {
            Storage.SaveSettings(_model.Settings);
            base.OnDisappearing();
        }
    }
}