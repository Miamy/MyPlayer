using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyPlayer.Models.Classes
{
    public class PathElement : IPathElement
    {
        public static string[] MusicExtentions => new string[] { ".mp3", ".flac" };
        //public static Dictionary<IPathElement, string> Hierarhy = new Dictionary<IPathElement, string>();
        public string Name { get; set; }

        public string FullPath { get; set; }
        public IPathElement Parent { get; set; }

        public bool IsFile => Name?.IndexOf(".") > -1;

        public string IconName => IsFile ? "baseline_audiotrack_black_36dp.png" : "baseline_folder_open_black_36dp.png";

        public bool IsVirtual => Parent == null;


        public bool IsComposite { get; set; }
        public bool HasDescription { get; set; }

        public string DescriptionFilename => Path.Combine(Path.GetDirectoryName(FullPath), Path.GetFileNameWithoutExtension(FullPath) + ".cue");

        public PathElement(string name, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Bad path {path}");
            }
            if (name == null)
            {
                var parts = PathElement.Split(path);
                Name = parts[^1];
            }
            else
            {
                Name = name;
            }
            FullPath = path;

            Parent = null;
            Init();
        }

        public PathElement(IPathElement parent, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Bad path {path}");
            }
            var parts = PathElement.Split(path);
            Name = parts[^1];
            FullPath = path;

            Parent = parent;
            Init();
        }

        private static readonly char[] Splitters = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        private static string[] Split(string path)
        {
            return path.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
        }


        public static string GetLevelUp(string path, int count)
        {
            var parts = PathElement.Split(path);
            if (parts.Length < count + 1)
            {
                return "";
            }
            return parts[^(count + 1)];

        }

        public override string ToString()
        {
            return FullPath;
        }

        public static bool IsMusicFile(string name)
        {
            var extention = Path.GetExtension(name);
            return (MusicExtentions.Any(ext => ext == extention));
        }

        private void Init()
        {
            HasDescription = IsComposite = Path.GetExtension(Name) == ".flac";
            if (IsComposite)
            {
                HasDescription = File.Exists(DescriptionFilename);
            }
        }
    }
}
