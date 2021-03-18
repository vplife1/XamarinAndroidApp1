

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.View;

namespace XamarinAndroidApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Shapes_Experimental", "MediaElement_Experimental" });

            MainPage = new NavigationPage(new Login());
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
