using News.Vampire.Models;
using System;
using Xamarin.Forms;

namespace News.Vampire.ViewModels
{
    public class SourceViewModel : BaseViewModel<Source>
    {
        private string text;
        private string url;
        private string description;
        private int sort;
        private string groupId;


        public SourceViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(url)
                && !String.IsNullOrWhiteSpace(description)
                && !String.IsNullOrWhiteSpace(groupId)
                && sort != 0;
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Url
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public int Sort
        {
            get => sort;
            set => SetProperty(ref sort, value);
        }

        public string GroupId
        {
            get => groupId;
            set => SetProperty(ref groupId, value);
        }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Source newSource = new Source()
            {
                Id = Guid.NewGuid().ToString(),
                Text = Text,
                Description = Description,
                Sort = Sort,
                Url = Url,
                GroupId = GroupId
            };

            await DataStore.AddAsync(newSource);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
