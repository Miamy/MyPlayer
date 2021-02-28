using System;
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;

using LibVLCSharp.Forms.Shared;
using MyPlayer.Models.Interfaces;
using MyPlayer.Droid;


namespace MyPlayer.Droid
{
    [Activity(Label = "MyPlayer", Icon = "@drawable/note", RoundIcon = "@drawable/round_note",
        Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { "android.intent.action.MAIN",
                          "android.intent.action.MUSIC_PLAYER",
                          "android.intent.category.LAUNCHER",
                          "android.intent.category.APP_MUSIC",
                          "android.intent.category.DEFAULT" })]

    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            LibVLCSharpFormsRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetPermissions();
            LoadApplication(new App());
        }


        #region Permissions
        private void SetPermissions()
        {
            List<string> permission = new List<string>(); ;

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted &&
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                permission.Add(Manifest.Permission.ReadExternalStorage);
                permission.Add(Manifest.Permission.WriteExternalStorage); ;
            }

            //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothPrivileged) != Permission.Granted && 
            //    ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothAdmin) != Permission.Granted)
            //{
            //    permission.Add(Manifest.Permission.Bluetooth);
            //    permission.Add(Manifest.Permission.BluetoothAdmin);
            //    permission.Add(Manifest.Permission.BluetoothPrivileged);
            //}

            if (permission.Count > 0)
                ActivityCompat.RequestPermissions(this, permission.ToArray(), 2);

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        #endregion



    }
}

/*
AbsMusicPlayerService	onStartCommand(intent(Intent { act=com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_SKIP_BACK cmp=com.onkyo.jp.musicplayer/.service.dap.DapMusicPlayerService (has extras) })= com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_SKIP_BACK,flags = 0, id = 11)
AbsMusicPlayerService	onStartCommand(intent(Intent { act=com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_PAUSE cmp=com.onkyo.jp.musicplayer/.service.dap.DapMusicPlayerService (has extras) })= com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_PAUSE,flags = 0, id = 9)
AbsMusicPlayerService	onStartCommand(intent(Intent { act=com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_PLAY_TOGGLE cmp=com.onkyo.jp.musicplayer/.service.dap.DapMusicPlayerService (has extras) })= com.onkyo.jp.musicplayer.MusicPlayerService.ACTION_PLAY_TOGGLE,flags = 0, id = 8)
*/