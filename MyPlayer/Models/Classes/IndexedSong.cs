using MyPlayer.Models.Interfaces;

namespace MyPlayer.Models.Classes
{
    public class IndexedSong
    {
        public IndexedSong(int index, ISong song)
        {
            Index = index;
            Song = song;
        }

        public int Index { get; set; }
        public ISong Song { get; set; }
    }
}
