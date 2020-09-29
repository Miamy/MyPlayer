using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.Models.Classes
{
    public class GraphicFile : PathElement, IGraphicFile
    {
        public GraphicFile(string name, string path) : base(name, path)
        {
        }
    }
}
