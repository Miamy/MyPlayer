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

        public static void SaveQueue(IQueueViewModel queue)
        {
            var folder = DependencyService.Get<IFileSystem>().GetWorkFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var filename = "query.json";
            var file = Path.Combine(folder, filename);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var settings = new JsonSerializerSettings()
            {
                MaxDepth = 6,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
            var formatter = Formatting.Indented;
            var value = JsonConvert.SerializeObject(queue, formatter, settings);
            File.WriteAllText(file, value);

            var zipFile = Path.Combine(folder, "query.zip");
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            using (var stream = new FileStream(zipFile, FileMode.Create))
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(file, filename);
                }
            }
            File.Delete(file);
        }

        public static void LoadQueue(IQueueViewModel queue)
        {

        }
    }
}
