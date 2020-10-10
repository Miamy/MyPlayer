using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IArtist : IMediaBase
    {
        void AddAlbum(IAlbum album);
    }
}
