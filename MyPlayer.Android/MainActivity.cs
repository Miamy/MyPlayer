﻿using System;
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
    [Activity(Label = "MyPlayer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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