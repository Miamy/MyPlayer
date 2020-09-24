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
    public class ArtistToHeightConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty ModelProperty = BindableProperty.Create("Model", typeof(object), typeof(ArtistToHeightConverter));

        public object Model
        {
            get { return GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        public List<IGrouping<IArtist, IGrouping<IAlbum, ISong>>> Songs { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var label = (Label)parameter;
            var name = label.Text;
            //var album = Songs.
            return 300;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (string)value == " " ? 0 : Parse(value);
            throw new NotImplementedException();

        }
    }



}
