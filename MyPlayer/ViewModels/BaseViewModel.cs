using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected int HeaderHeight { get; set; } = 40;
        protected int ItemHeight { get; set; } = 30;

        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
