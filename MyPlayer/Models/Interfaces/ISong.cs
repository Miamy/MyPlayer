using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{ 
    public interface ISong : IMediaBase
    {
        TimeSpan Duration { get; set; }
        IAlbum Album { get; set; }
        TimeSpan OffcetInContainer { get; set; }

        
    }
}
