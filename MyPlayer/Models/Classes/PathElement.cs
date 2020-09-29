using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPlayer.Models.Classes
{
    public class PathElement : IPathElement
    {
        //public static Dictionary<IPathElement, string> Hierarhy = new Dictionary<IPathElement, string>();
        public string Name { get; set; }

        public string FullPath { get; set; }
        public IPathElement Parent { get; set; }

        public bool IsFile => Name?.IndexOf(".") > -1;

        public string IconName => IsFile ? "baseline_audiotrack_black_36dp.png" : "baseline_folder_open_black_36dp.png";

        public PathElement(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            var parts = PathElement.Split(path);
            Name = parts[^1];
            FullPath = path;
            
            //Parent = GetParent(path);
        }

        private static readonly char[] Splitters = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        private static string[] Split(string path)
        {
            return path.Split(Splitters, StringSplitOptions.RemoveEmptyEntries); 
        }


        public static string GetParent(string path)
        {
            var parts = PathElement.Split(path);
            if (parts.Length < 2)
            {
                return "";
            }
            return parts[^2];

        }
        public static string GetGrandParent(string path)
        {
            var parts = PathElement.Split(path);
            if (parts.Length < 3)
            {
                return "";
            }
            return parts[^3];
        }
    }

}
