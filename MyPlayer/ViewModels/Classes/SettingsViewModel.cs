using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class SettingsViewModel : BaseModel
    {
        public ICommand SelectRootCommand { get; set; }
        public ISettings Settings { get; set; }

        public SettingsViewModel(ISettings settings)
        {
            //if (settings == null)
            //    settings = Settings.Instance;
            Settings = settings;
            CreateCommands();
        }

        #region Commands
        private void CreateCommands()
        {
            SelectRootCommand = new Command(SelectRootAction, CanSelectRoot);
        }

        private bool CanSelectRoot(object arg)
        {
            return true;
        }

        private async void SelectRootAction(object obj)
        {
            var page = new BrowsePage();
            await Application.Current.MainPage.Navigation.PushAsync(page, false);
        }
        #endregion

     
    }
}
