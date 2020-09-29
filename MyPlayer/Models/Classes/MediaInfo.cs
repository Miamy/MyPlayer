using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class MediaInfo : IMediaInfo
    {
        private string _albumName;

        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                if (_albumName.Length < 4)
                {
                    return;
                }
                var year = _albumName.Substring(0, 4);
                if (int.TryParse(year, out int test))
                {
                    Year = test;
                    _albumName = _albumName.Substring(6, _albumName.Length - 6);
                }
            }
        }
        public int Year { get; set; }
        public IGraphicFile Cover { get; set; }
        public string Artist { get; set; }

        public MediaInfo()
        {
            Clear();
        }

        public void Clear()
        {
            AlbumName = "";
            Artist = "";
            Year = 0;
            Cover = null;
        }

        public void Setup(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            AlbumName = PathElement.GetParent(path);
            Artist = PathElement.GetGrandParent(path);

            var dir = Path.GetDirectoryName(path);
            var files = PathScanner.GetCovers(dir);
            if (files.Length > 0)
            {
                Cover = new GraphicFile(null, files[0]);
            }
        }
    }
}
