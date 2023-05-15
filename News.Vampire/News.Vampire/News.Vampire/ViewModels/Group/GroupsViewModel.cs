using News.Vampire.Models;
using News.Vampire.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace News.Vampire.ViewModels
{
    public class GroupsViewModel : BaseViewModel<Group>
    {
        private Group _selectedGroup;

        public ObservableCollection<Group> Groups { get; }
        public Command LoadGroupsCommand { get; }
        public Command AddGroupCommand { get; }
        public Command<Group> GroupTapped { get; }

        public GroupsViewModel()
        {
            Title = "Browse groups";
            Groups = new ObservableCollection<Group>();
            LoadGroupsCommand = new Command(async () => await ExecuteLoadGroupsCommand());

            GroupTapped = new Command<Group>(OnGroupSelected);
        }

        async Task ExecuteLoadGroupsCommand()
        {
            IsBusy = true;

            try
            {
                Groups.Clear();
                var items = await DataStore.GetAsync(true);
                foreach (var item in items)
                {
                    Groups.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedGroup = null;
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                OnGroupSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnGroupSelected(Group group)
        {
            if (group == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={group.Id}");
        }
    }
}
