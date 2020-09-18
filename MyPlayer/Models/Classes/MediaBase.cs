using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class MediaBase : IMediaBase
    { 
        public IMediaInfo MediaInfo { get; set; }
        
        public MediaBase()
        {
            MediaInfo = new MediaInfo();
        }       
    }
}
