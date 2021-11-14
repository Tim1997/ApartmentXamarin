using ApertmantXamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ApertmantXamarin.ViewModels
{
    public class HomeViewModel: BaseViewModel
    {
        #region Command 
        public ICommand AddCommand => new Command(async () =>
        {
            Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[1];
        });

        public ICommand ListCommand => new Command(async () =>
        {
            Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[2];
        });

        public ICommand InfoCommand => new Command(async () =>
        {
            Shell.Current.CurrentItem = Shell.Current.CurrentItem.Items[3];
        });

        #endregion
        public HomeViewModel()
        {
            Title = "Home";
        }
    }
}
