using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyPlayer.Models
{
    class ArtistHeightConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty MyArtistProperty = BindableProperty.Create(nameof(MyArtist), typeof(string), typeof(ArtistHeightConverter));

        public string MyArtist
        {
            get { return (string)GetValue(MyArtistProperty); }
            set { SetValue(MyArtistProperty, value); }
        }
        //public object Artist { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var label = (Label)parameter;
            //var name = label.Text;
            //var album = Songs.
            return 300;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class AlbumHeightConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty MyAlbumProperty = BindableProperty.Create(nameof(MyAlbum), typeof(object), typeof(AlbumHeightConverter));

        public object MyAlbum
        {
            get { return GetValue(MyAlbumProperty); }
            set { SetValue(MyAlbumProperty, value); }
        }
        //public object Album { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var label = (Label)parameter;
            //var name = label.Text;
            //var album = Songs.
            return 300;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
