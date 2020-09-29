using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;

namespace MyPlayer.ViewModels
{
    public class SongModel: SelectionViewModel
    {
        public int Index { get; set; }
        private ISong _song;

        public string Name => _song.Name;
        public int SongHeight => ItemHeight;
        public SongModel(ISong song)
        {
            _song = song;
        }     

    }
}
