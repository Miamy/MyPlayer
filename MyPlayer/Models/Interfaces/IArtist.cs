using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IArtist
    {
        string Name { get; set; }
        IList<IAlbum> Albums { get; set; }
        int Count { get; }
        void AddAlbum(IAlbum album);
    }
}
