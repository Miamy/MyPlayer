using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using MyPlayer.Models.Interfaces;

namespace MyPlayer.Models.Classes
{
    public class ChartLyricsLyricsFinder : ILyricsFinder
    {
        private const string SearchAddress = "http://api.chartlyrics.com/apiv1.asmx/SearchLyricDirect?artist={0}&song={1}";
        

        public async Task<string> FindLyricsAsync(string artist, string song)
        {
            if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(song))
            {
                throw new ArgumentException("Value cannot be blank");
            }
            var index = song.IndexOf("-");
            if (index > -1)
            {
                song = song.Substring(index + 1);
            }

            var uri = new Uri(string.Format(SearchAddress, artist.Trim(), song.Trim()));
            var request = (HttpWebRequest)WebRequest.Create(uri);

            using var response = await request.GetResponseAsync();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            
            var data = await reader.ReadToEndAsync();
            var xml = XDocument.Parse(data);

            try
            {
                var lyrics = xml.Root.Elements().Where(e => e.Name.LocalName == "Lyric").First().Value;
                return lyrics;
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
