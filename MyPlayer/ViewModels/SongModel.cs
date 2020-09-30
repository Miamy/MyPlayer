using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;

namespace MyPlayer.ViewModels
{
    public class SongModel: SelectionViewModel
    {
        public int Index { get; set; }

        public string Name => Song.Name;
        public int SongHeight => ItemHeight;

        public ISong Song { get; set; }

        public SongModel(ISong song)
        {
            Song = song;
        }


        public override string ToString()
        {
            return Song.ToString();
        }
    }
}
