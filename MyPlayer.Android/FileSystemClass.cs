using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyPlayer.Droid;
using MyPlayer.Models.Interfaces;


[assembly: Xamarin.Forms.Dependency(typeof(FileSystemClass))]
namespace MyPlayer.Droid
{
    public class FileSystemClass : IFileSystem
    {
        [Obsolete]
        public static string GetWorkFolderStatic()
        {
            var s = Android.OS.Environment.DirectoryDocuments;
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "MyPlayer");
        }
        
        [Obsolete]
        public string GetWorkFolder()
        {
            return GetWorkFolderStatic();
        }

    }
}