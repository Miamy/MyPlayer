using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPlayer.Models.Classes
{
    public class PathElement : IPathElement
    {
        public static Dictionary<IPathElement, string> Hierarhy = new Dictionary<IPathElement, string>();
        public string Name { get; set; }

        public string FullPath { get; set; }
        public IPathElement Parent { get; set; }

        public PathElement(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            var parts = PathElement.Split(path);
            Name = parts[parts.Length - 1];
            FullPath = path;
        }

        private static string[] Split(string path)
        {
            return path.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries); 
        }


        public static string GetParent(string path)
        {
            var parts = PathElement.Split(path);
            if (parts.Length < 2)
            {
                return "";
            }
            return parts[parts.Length - 2];

        }
        public static string GetGrandParent(string path)
        {
            var parts = PathElement.Split(path);
            if (parts.Length < 3)
            {
                return "";
            }
            return parts[parts.Length - 3];
        }
    }

}
