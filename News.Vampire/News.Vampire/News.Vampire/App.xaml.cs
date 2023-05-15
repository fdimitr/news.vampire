using News.Vampire.Services;
using News.Vampire.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News.Vampire
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockItemDataStore>();
            MainPage = new AppShell();
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
