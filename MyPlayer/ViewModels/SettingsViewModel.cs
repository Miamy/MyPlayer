using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string aName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aName));
        }
    }
}
