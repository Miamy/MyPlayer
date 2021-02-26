using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPlayer.Models.Interfaces
{
    public interface ILyricsFinder
    {
        StringBuilder FindLyrics(string artist, string song);
        Task<StringBuilder> FindLyricsAsync(string artist, string song);
    }
}
