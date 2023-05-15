using News.Vampire.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace News.Vampire.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}