using MyPlayer.CommonClasses;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class Settings : BaseModel, ISettings
    {
        private string _rootFolder;

        public string RootFolder
        {
            get => _rootFolder;
            set => Set(ref _rootFolder, value);
        }


        public Settings()
        {
            
        }

    }
}
