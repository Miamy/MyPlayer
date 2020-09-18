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
    }
}
