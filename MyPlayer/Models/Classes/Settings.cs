using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Settings : BaseViewModel, ISettings
    {
        private string _rootFolder;

        public string RootFolder
        {
            get => _rootFolder;
            set => Set(ref _rootFolder, value);
        }


        private Settings()
        {
            Storage.LoadSettings(this);
        }

        private static ISettings _instance;
        public static ISettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
    }
}
