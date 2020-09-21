using MyPlayer.Models.Interfaces;

namespace MyPlayer.Models.Classes
{
    public class SongModel
    {
        public int Index { get; set; }
        public ISong Song { get; set; }
        public SongModel(int index, ISong song)
        {
            Index = index;
            Song = song;
        }     
    }
}
