using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPlayer.Models.Interfaces
{
    public interface ILyricsFinder
    {
        Task<string> FindLyricsAsync(string artist, string song);
    }
}
