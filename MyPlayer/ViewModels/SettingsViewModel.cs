using MyPlayer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _rootFolder;

        public string RootFolder { get => _rootFolder; set => Set(ref _rootFolder, value); }

        public ICommand SelectRootCommand { get; set; }

        public SettingsViewModel()
        {
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
