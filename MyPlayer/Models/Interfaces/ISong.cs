using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{ 
    public interface ISong : IMediaBase
    {
        string Name { get; set; }
        TimeSpan Duration { get; set; }
        IFile Container { get; set; }
        IAlbum Album { get; set; }
        TimeSpan OffcetInContainer { get; set; }
    }
}
