using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MyPlayer.ViewModels
{
    public class BrowseViewModel : BaseViewModel
    {
        public string Current { get; set; }

        public List<RootInfo> RootFolders { get; set; }

        public BrowseViewModel()
        {
            BuildRoot();
        }

        private void BuildRoot()
        {
            RootFolders = new List<RootInfo>
            {
                new RootInfo("Internal storage", "")
            };

            var storages = Environment.GetLogicalDrives().Where(drive => drive.IndexOf("storage") > 0).ToList();
            if (storages.Count == 0)
            {
                return;
            }
            RootFolders[0].Path = storages[0];
            if (storages.Count > 1)
            {
                RootFolders.Add(new RootInfo("SD card", storages[1]));
            }
        }
    }

    public class RootInfo
    {  
        public string Name { get; set; }
        public string Path { get; set; }
        public RootInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }
    
    }
}
