using MyPlayer.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyPlayer.ViewModels
{
    public interface IQueueViewModel
    {
        IReadOnlyCollection<VisualObject<IArtist>> Artists { get; }
        IQueue Queue { get; set; }

        ISong Next(ISong song);
        ISong Prev(ISong song);
    }
}