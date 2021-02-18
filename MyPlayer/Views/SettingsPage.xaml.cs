using MyPlayer.CommonClasses;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
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

        public SettingsPage(ISettings settings)
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel(settings);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Storage.LoadSettings(_model.Settings);
        }

        protected override void OnDisappearing()
        {
            //Storage.SaveSettings(_model.Settings);
            base.OnDisappearing();
        }
    }
}