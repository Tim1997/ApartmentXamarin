using ApertmantXamarin.Models;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ApertmantXamarin.ViewModels
{
    public class NewPropertyModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<string> cities;
        private Item apartment;
        private int index;

        public ObservableCollection<string> Cities { get => cities; set => SetProperty(ref cities, value); }
        public Item Apartment { get => apartment; set => SetProperty(ref apartment, value); }
        public int Index { get => index; set => SetProperty(ref index, value); }


        #endregion

        #region Command
        public ICommand CreateCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Apartment.Username?.Trim())
            || string.IsNullOrEmpty(Apartment.Phone)
            || Apartment.Price == 0 || Apartment.RoomNumber == 0
            || Index == -1
            || Apartment.DateHire.Date == DateTime.MinValue.Date)
            {
                await Shell.Current.DisplayAlert("Alert", "Value can not be empty or null", "Got it");
            }
            else
            {
                Apartment.City = (City)Index;

                var itemExist = (await FirebaseDatabase.Child("Apartments")
                .OnceAsync<Item>()).FirstOrDefault(x => x.Object.RoomNumber == Apartment.RoomNumber
                && x.Object.City == Apartment.City);
                if (itemExist != null)
                {
                    await Shell.Current.DisplayAlert("Alert", "Room is booked", "Got it");
                }
                else
                {
                    await FirebaseDatabase.Child("Apartments")
                    .PostAsync(Apartment);

                    Index = -1;
                    Apartment = new Item();
                    await Shell.Current.DisplayAlert("Alert", "Create Apartment success", "Got it");
                }
            }
        });

        #endregion


        public NewPropertyModel()
        {
            Init();
            /////////////////////////
        }

        #region Method
        void Init()
        {
            Title = "New Apartment";
            Index = -1;
            Cities = new ObservableCollection<string>(Enum.GetValues(typeof(City)).Cast<City>()
                .Select(x => Description(x)));
            Apartment = new Item();
        }

        string Description(Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        #endregion
    }
}
