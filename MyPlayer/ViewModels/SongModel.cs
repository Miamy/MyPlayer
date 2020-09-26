using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;

namespace MyPlayer.ViewModels
{
    public class SongModel: SelectionViewModel
    {
        public int Index { get; set; }
        private ISong _song;

        public string Name => _song.Name;
        public int Height => ItemHeight;
        public SongModel(int index, ISong song)
        {
            Index = index;
            _song = song;
        }     
    }
}
