using MyPlayer.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.Models.Classes
{
    public static class Storage
    {
        public static void SaveSettings(ISettings settings)
        {
            Preferences.Set("RootFolder", settings.RootFolder);
        }

        public static void LoadSettings(ISettings settings)
        {
            settings.RootFolder = Preferences.Get("RootFolder", "");
        }

        public static void SaveQueue(IQueue queue)
        {
            var settings = new JsonSerializerSettings()
            {
                MaxDepth = 2,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
            var formatter = Formatting.Indented;
            var value = JsonConvert.SerializeObject(queue, formatter, settings);
            var folder = DependencyService.Get<IFileSystem>().GetWorkFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var file = Path.Combine(folder, "query.txt");
            File.WriteAllText(file, value);
            using (StreamReader reader = new StreamReader()
            var zip = new ZipArchive()

        }
        public static void LoadQueue(IQueue queue)
        {

        }
    }
}
