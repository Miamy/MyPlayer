using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IMediaInfo
    {
        string AlbumName { get; set; }
        int Year { get; set; }
        IGraphicFile Cover { get; set; }
        string Artist { get; set; }

        void Clear();
        void Setup(string path);
    }
}
