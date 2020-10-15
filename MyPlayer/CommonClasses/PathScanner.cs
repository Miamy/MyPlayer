using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyPlayer.CommonClasses
{
    public static class PathScanner
    {
        public static string[] GraphicFiles => new string[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "svg" };
        public static string[] MusicFiles => new string[] { "mp3", "flac" };

        public static string[] GetFilesFrom(string searchFolder, string[] filters, bool isRecursive)
        {
            var filesFound = new List<string>();
            if (Directory.Exists(searchFolder))
            {
                var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                foreach (var filter in filters)
                {
                    filesFound.AddRange(Directory.GetFiles(searchFolder, string.Format("*.{0}", filter), searchOption));
                }
            }
            return filesFound.ToArray();
        }

        public static string[] ProceedRoot(string root)
        {
            return GetFilesFrom(root, MusicFiles, true);
        }
        public static string[] GetCovers(string searchFolder)
        {
            return GetFilesFrom(searchFolder, GraphicFiles, false);
        }
    }
}
