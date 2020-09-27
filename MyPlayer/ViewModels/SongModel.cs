using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;

namespace MyPlayer.ViewModels
{
    public class SongModel: SelectionViewModel
    {
        public static int TotalIndex = 0;
        public int Index { get; set; }
        private ISong _song;

        public string Name => _song.Name;
        public int SongHeight => ItemHeight;
        public SongModel(int index, ISong song)
        {
            //Index = TotalIndex;
            Index = index;
            TotalIndex++;
            _song = song;
        }     
    }
}
