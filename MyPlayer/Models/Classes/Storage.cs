using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

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

        }
        public static void LoadQueue(IQueue queue)
        {

        }
    }
}
