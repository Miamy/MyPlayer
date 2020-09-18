using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IMusicFile : IFile
    {
        //MusicFileType Type { get; set; }
        bool IsComposite { get; set; }
        bool HasDescription { get; set; }
        string DescriptionFilename { get; }
    }
}
