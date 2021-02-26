using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyPlayer.Models.Interfaces;

namespace MyPlayer.Models.Classes
{
    public class ChartLyricsLyricsFinder : ILyricsFinder
    {
        public StringBuilder FindLyrics(string artist, string song)
        {
            throw new NotImplementedException();
        }

        public async Task<StringBuilder> FindLyricsAsync(string artist, string song)
        {
            throw new NotImplementedException();
        }
    }
}
