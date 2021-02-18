using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowsePage : ContentPage
    {
        private BrowseViewModel _model;
        public BrowsePage(ISettings settings)
        {
            InitializeComponent();
            BindingContext = _model = new BrowseViewModel(settings);
        }

    }
}