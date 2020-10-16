﻿using MyPlayer.Models.Classes;
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
            Preferences.Set("LoopType", (int)queue.LoopType);
            Preferences.Set("ShowAlbums", queue.ShowAlbums);
            Preferences.Set("ShowSongs", queue.ShowSongs);

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

            var queueData = new Dictionary<string, bool>();

            var simples = queue.Artists.Select(artist => new { artist.Data.Container, artist.IsSelected });
            foreach (var element in simples)
            {
                queueData.Add(element.Container, element.IsSelected);
            }

            var albums = queue.Artists.SelectMany(artist => artist.Children);
            simples = albums.Select(album => new { album.Data.Container, album.IsSelected });
            foreach (var element in simples)
            {
                queueData.Add(element.Container, element.IsSelected);
            }

            var songs = albums.SelectMany(albums => albums.Children);
            simples = songs.Select(song => new { Container = ((ISong)song.Data).GetUniqueName(), song.IsSelected });
            foreach (var element in simples)
            {
                queueData.Add(element.Container, element.IsSelected);
            }

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

        public static void LoadQueue(IQueueViewModel queue)
        {
            if (queue == null)
            {
                return;
            }

            var folder = DependencyService.Get<IFileSystem>().GetWorkFolder();
            if (!Directory.Exists(folder))
            {
                return;
            }

            FileAttributes attributes = File.GetAttributes(folder);
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

            using (var stream = new FileStream(zipFile, FileMode.Open))
            {
                using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
                zip.ExtractToDirectory(folder, true);
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
                //var queue = JsonConvert.DeserializeObject<IQueueViewModel>(value, GetSettings());
                var queueData = JsonConvert.DeserializeObject<Dictionary<string, bool>>(value, GetSettings());
                foreach (var data in queueData)
                {
                    var item = queue.Artists.Flatten(e => e.Children).FirstOrDefault(d => d.Data.GetUniqueName() == data.Key);
                    if (item != null)
                    {
                        item.IsSelected = data.Value;
                    }
                }

            }
            catch (Exception e)
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

        //public static Func<IReferenceResolver> IDReferenceResolver { get; set; } 
    }
}
