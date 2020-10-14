using LibVLCSharp.Shared;
using MyPlayer.Models.Classes;
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
        private MainWindowViewModel Model { get; set; }
        private bool Execute { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = Model = new MainWindowViewModel();
        }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Execute = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
            {
                //NameLabelHidden.IsVisible = true;
                if (NameLabel.Width > InfoLayout.Width / 2 - 10)
                {
                    NameLabel.TranslationX += 4;
                    //NameLabelHidden.TranslationX += 204;

                    if (NameLabel.TranslationX > InfoLayout.Width )
                    {
                        NameLabel.TranslationX = -NameLabel.Width;
                    }
                    //if (NameLabelHidden.TranslationX > InfoLayout.Width)
                    //{
                    //    NameLabelHidden.TranslationX = -NameLabelHidden.Width;
                    //}
                }
                else
                {
                    NameLabel.TranslationX = 0;
                    //NameLabelHidden.TranslationX = InfoLayout.Width / 2;
                    //NameLabelHidden.IsVisible = false;
                }
                return Execute;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Execute = false;
        }
    }
}
