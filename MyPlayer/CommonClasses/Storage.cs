using MyPlayer.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.CommonClasses
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

            //var formatter = Formatting.Indented;
            //var value = JsonConvert.SerializeObject(queue, formatter, GetSettings());

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Include,
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                MaxDepth = 116,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            using (StreamWriter sw = new StreamWriter(file))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, queue, typeof(IQueueViewModel));
            }

            //File.WriteAllText(file, value);

            var zipFile = Path.Combine(folder, "query.zip");
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            using (var stream = new FileStream(zipFile, FileMode.Create))
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Create);
                zip.CreateEntryFromFile(file, filename);
            }
            File.Delete(file);
        }

        public static IQueueViewModel LoadQueue()
        {
            var folder = DependencyService.Get<IFileSystem>().GetWorkFolder();
            if (!Directory.Exists(folder))
            {
                return null;
            }

            var zipFile = Path.Combine(folder, "query.zip");
            if (!File.Exists(zipFile))
            {
                return null;
            }

            using (var stream = new FileStream(zipFile, FileMode.Open))
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    zip.ExtractToDirectory(folder, true);
                }
            }

            var filename = "query.json";
            var file = Path.Combine(folder, filename);
            if (!File.Exists(file))
            {
                return null;
            }

            var value = File.ReadAllText(file);
            try
            {
                var queue = JsonConvert.DeserializeObject<IQueueViewModel>(value, GetSettings());
                return queue;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                File.Delete(file);
            }
        }

        private static JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings()
            {
                MaxDepth = 116,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                NullValueHandling = NullValueHandling.Include,
                TypeNameHandling = TypeNameHandling.Auto,

                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceResolver = new IDReferenceResolver()
        };
        }

        //public static Func<IReferenceResolver> IDReferenceResolver { get; set; } 
    }
}
