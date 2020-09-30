using System.ComponentModel;

namespace MyPlayer.Models.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        string RootFolder { get; set; }
    }
}