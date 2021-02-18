using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer
{
    public partial class App : Application
    {
        public static INavigation Navigation => Application.Current.MainPage.Navigation;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("#181818"),
            //BarTextColor = Color.White
            };
            ;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
