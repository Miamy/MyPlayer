using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class MusicFile : PathElement, IMusicFile
    { 
        public bool IsComposite { get; set; }
        public bool HasDescription { get; set; }
        
        public string DescriptionFilename => Path.Combine(Path.GetDirectoryName(FullPath), Path.GetFileNameWithoutExtension(FullPath) + ".cue");
      

        public MusicFile(string name, string path) : base(name, path)
        {
            HasDescription = IsComposite = Path.GetExtension(path) == ".flac";
            if (IsComposite)
            {
                HasDescription = File.Exists(DescriptionFilename);
            }
        }
    }
}
