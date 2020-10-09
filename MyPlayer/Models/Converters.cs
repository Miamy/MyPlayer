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
    public class ArtistHeightConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty ArtistProperty = BindableProperty.Create(nameof(Artist), typeof(object), typeof(ArtistHeightConverter));

        public object Artist
        {
            get { return GetValue(ArtistProperty); }
            set { SetValue(ArtistProperty, value); }
        }

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

    public class AlbumHeightConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty AlbumProperty = BindableProperty.Create(nameof(Album), typeof(object), typeof(AlbumHeightConverter));

        public object Album
        {
            get { return GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

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
