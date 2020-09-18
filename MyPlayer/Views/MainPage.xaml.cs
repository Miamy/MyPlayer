using MyPlayer.ViewModels;
using MyPlayer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyPlayer
{
    public partial class MainPage : ContentPage
    {
        //public MainWindowViewModel Model { get; set; }

        public MainPage()
        {
            InitializeComponent();

            //BindingContext = Model = new MainWindowViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

       
    }
}
