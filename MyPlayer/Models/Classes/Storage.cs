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
            var fullFilename = Path.Combine(folder, filename);
            if (File.Exists(fullFilename))
            {
                File.Delete(fullFilename);
            }

            var queueData = queue.Artists.Flatten(a => a.Children).Where(a => !a.IsSelected).ToDictionary(a => a.GetUniqueName(), a => a.IsSelected);

            var formatter = Formatting.Indented;
            var value = JsonConvert.SerializeObject(queueData, formatter, GetSettings());
            File.WriteAllText(fullFilename, value);


            var zipFilename = Path.Combine(folder, "query.zip");
            if (File.Exists(zipFilename))
            {
                File.Delete(zipFilename);
            }
            using (var stream = new FileStream(zipFilename, FileMode.Create))
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Create);
                zip.CreateEntryFromFile(fullFilename, filename);
            }
            File.Delete(fullFilename);
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

            var zipFilename = Path.Combine(folder, "query.zip");
            if (!File.Exists(zipFilename))
            {
                return;
            }

            try
            {
                using var stream = new FileStream(zipFilename, FileMode.Open);
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                zip.ExtractToDirectory(folder, true);
            }
            catch (Exception)
            {
            }

            var filename = "query.json";
            var fullFilename = Path.Combine(folder, filename);
            if (!File.Exists(fullFilename))
            {
                return;
            }

            var value = File.ReadAllText(fullFilename);
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
                File.Delete(fullFilename);
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
