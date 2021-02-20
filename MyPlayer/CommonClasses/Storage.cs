using MyPlayer.Models;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.CommonClasses
{
    public class Storage : IStorage
    {
        public void SaveSettings(ISettings settings)
        {
            Preferences.Set("RootFolder", settings.RootFolder);
        }

        public void LoadSettings(ISettings settings)
        {
            settings.RootFolder = Preferences.Get("RootFolder", "");
        }

        public void SaveQueue(IQueue queue)
        {
            if (queue == null)
            {
                return;
            }
            Preferences.Set("LoopType", (int)queue.LoopType);
            Preferences.Set("Current", queue.Current.GetUniqueName());

            if (queue.Artists == null)
            {
                return;
            }

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

            var queueData = queue.Artists.Flatten(a => a.Children).Where(a => !a.IsSelected).ToDictionary(a => a.GetUniqueName(), a => a.IsSelected);

            var formatter = Formatting.Indented;
            var value = JsonConvert.SerializeObject(queueData, formatter, GetSettings());
            File.WriteAllText(file, value);


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

        public void LoadQueue(IQueue queue)
        {
            if (queue == null)
            {
                return;
            }

            queue.LoopType = (LoopType)Preferences.Get("LoopType", (int)LoopType.All);
            var currentName = Preferences.Get("Current", null);
            if (currentName != null)
            {
                queue.Current = queue.Songs?.FirstOrDefault(song => song.GetUniqueName() == currentName);
            }

            var folder = DependencyService.Get<IFileSystem>().GetWorkFolder();
            if (!Directory.Exists(folder))
            {
                return;
            }

            var attributes = File.GetAttributes(folder);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes &= ~FileAttributes.ReadOnly;
                File.SetAttributes(folder, attributes);
            }

            var zipFile = Path.Combine(folder, "query.zip");
            if (!File.Exists(zipFile))
            {
                return;
            }

            try
            {
                using var stream = new FileStream(zipFile, FileMode.Open);
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                zip.ExtractToDirectory(folder, true);
            }
            catch (Exception)
            {
            }

            var filename = "query.json";
            var file = Path.Combine(folder, filename);
            if (!File.Exists(file))
            {
                return;
            }

            var value = File.ReadAllText(file);
            try
            {
                var flattened = queue.Artists.Flatten(a => a.Children);
                var queueData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value, GetSettings());
                foreach (var data in queueData)
                {
                    var item = flattened.FirstOrDefault(d => d.GetUniqueName() == data.Key);
                    if (item != null)
                    {
                        item.IsSelected = data.Value;
                    }
                }
            }
            catch (Exception)
            {
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
                //ReferenceResolver = new IDReferenceResolver()
            };
        }
    }
}
