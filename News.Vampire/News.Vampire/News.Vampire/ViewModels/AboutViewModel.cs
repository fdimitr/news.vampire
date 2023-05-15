using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace News.Vampire.ViewModels
{
    public class AboutViewModel : BaseViewModel<Object>
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}