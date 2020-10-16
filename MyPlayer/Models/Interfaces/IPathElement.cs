using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.Models.Interfaces
{
    public interface IPathElement
    {
        string Name { get; set; }
        string FullPath { get; }
        IPathElement Parent { get; set; }

        bool IsFile { get; }
        bool IsVirtual { get; }
        bool IsComposite { get; set; }
        bool HasDescription { get; set; }
        string DescriptionFilename { get; }
    }
}