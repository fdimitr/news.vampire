using News.Vampire.Models;
using System;
using Xamarin.Forms;

namespace News.Vampire.ViewModels
{
    public class GroupDetailViewModel : BaseViewModel<Group>
    {
        private string name;
        private bool isActive;

        public GroupDetailViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool IsActiverl
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
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
            Group newSource = new Group()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                isActive = isActive
            };

            await DataStore.AddAsync(newSource);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
