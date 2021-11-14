using ApertmantXamarin.Models;
using Newtonsoft.Json;
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
    public class FilterViewModel: BaseViewModel
    {
        #region Properties
        private Item filterApartment; 
        private ObservableCollection<string> cities;
        private int index;

        public Item FilterApartment { get => filterApartment; set => SetProperty(ref filterApartment, value); }
        public ObservableCollection<string> Cities { get => cities; set => SetProperty(ref cities, value); }
        public int Index { get => index; set => SetProperty(ref index, value); }
        #endregion
        #region Command
        public ICommand SearchCommand => new Command(async () =>
        {
            if(Index != -1)
            {
                FilterApartment.City = (City)Index;
            }

            await Shell.Current.GoToAsync($"..?{nameof(PropertyListViewModel.Parameter)}={ JsonConvert.SerializeObject(FilterApartment)}");
        });
        #endregion

        public FilterViewModel()
        {
            Init();
            ////////////////////
            
        }

        #region Method
        void Init()
        {
            Title = "Filter";
            Index = -1;
            Cities = new ObservableCollection<string>(Enum.GetValues(typeof(City)).Cast<City>()
                .Select(x => Description(x)));
            FilterApartment = new Item();
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
