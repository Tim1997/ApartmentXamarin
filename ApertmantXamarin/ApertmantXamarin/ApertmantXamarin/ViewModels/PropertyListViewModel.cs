using ApertmantXamarin.Models;
using ApertmantXamarin.Views;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ApertmantXamarin.ViewModels
{
    [QueryProperty(nameof(Parameter), nameof(Parameter))]
    public class PropertyListViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<Item> apartments;
        private string parameter;
        private Item filterApartment;
        private string filterUsername;

        public ObservableCollection<Item> Apartments { get => apartments; set => SetProperty(ref apartments, value); }
        public Item FilterApartment { get => filterApartment; set => SetProperty(ref filterApartment, value); }
        public string FilterUsername 
        { 
            get => filterUsername;
            set
            {
                filterUsername = value;
                FilterChangedCommand.Execute(filterUsername);
            }
        }

        public string Parameter
        {
            get => parameter;
            set
            {
                parameter = Uri.UnescapeDataString(value ?? string.Empty);
                SetProperty(ref parameter, value);

                if (!string.IsNullOrEmpty(parameter))
                {
                    FilterApartment = JsonConvert.DeserializeObject<Item>(parameter);
                    ExecuteFilterItems();
                }
            }
        }
        #endregion

        #region Command 
        public ICommand FilterCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(FilterPage)}");
        });

        public ICommand RemoveItemCommand => new Command<Item>(async (aparment) =>
        {
            var result = await Shell.Current.DisplayActionSheet("Warning", "Cancel", "Yes", "Are you sure want remove item?");
            if (result == "Yes")
            {
                try
                {
                    var item = (await FirebaseDatabase.Child("Apartments")
                                        .OnceAsync<Item>()).Where(x => x.Object.Id == aparment.Id).FirstOrDefault();

                    await FirebaseDatabase.Child("Apartments").Child(item?.Key).DeleteAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
        });

        public ICommand LoadItemsCommand { get; set; }
        public ICommand ResetCommand => new Command(async () => await ExecuteLoadClassroomsCommand());
        public ICommand FilterChangedCommand => new Command<string>(async (username) => await TextChanged(username));
        #endregion

        public PropertyListViewModel()
        {
            Title = "Apartment List";
            IsBusy = true;
            Apartments = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadClassroomsCommand());
        }

        #region Method
        async Task ExecuteLoadClassroomsCommand()
        {
            IsBusy = true;

            try
            {
                Apartments.Clear();
                var list = (await FirebaseDatabase.Child("Apartments")
                    .OnceAsync<Item>()).Select(x => new Item
                    {
                        Username = x.Object.Username,
                        City = x.Object.City,
                        DateHire = x.Object.DateHire,
                        Note = x.Object.Note,
                        Phone = x.Object.Phone,
                        Price = x.Object.Price,
                        RoomNumber = x.Object.RoomNumber,
                        TimeHire = x.Object.TimeHire,
                    });

                if(Apartments.Count == 0)
                {
                    foreach (var item in list)
                    {
                        Apartments.Add(item);
                    }
                }

                ExecuteFilterItems();
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

        Task TextChanged(string username)
        {
            // username
            var un = username?.Trim().ToLower();
            if (!string.IsNullOrEmpty(un))
            {
                var list = Apartments.ToList();
                list = list.Where(x => x.Username.Trim().ToLower().Contains(un)).ToList();

                Apartments.Clear();
                foreach (var item in list)
                {
                    Apartments.Add(item);
                }
            }

            return Task.CompletedTask;
        }

        void ExecuteFilterItems()
        {
            if (FilterApartment == null) return;

            var list = Apartments.ToList();

            // username
            var un = FilterApartment.Username?.Trim().ToLower();
            if (!string.IsNullOrEmpty(un))
            {
                list = list.Where(x => x.Username.Trim().ToLower().Contains(un)).ToList();
            }

            // phone
            var phone = FilterApartment.Phone?.Trim();
            if (!string.IsNullOrEmpty(phone))
            {
                list = list.Where(x => x.Phone == phone).ToList();
            }

            // roomnumber
            var room = FilterApartment.RoomNumber;
            if (room != 0)
            {
                list = list.Where(x => x.RoomNumber == room).ToList();
            }

            // city
            var city = FilterApartment.City;
            if (city != null)
            {
                list = list.Where(x => x.City == city).ToList();
            }

            Apartments.Clear();
            foreach (var item in list)
            {
                Apartments.Add(item);
            }
        }
        #endregion
    }
}
