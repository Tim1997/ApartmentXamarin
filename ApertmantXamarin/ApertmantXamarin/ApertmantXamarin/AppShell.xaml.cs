using ApertmantXamarin.ViewModels;
using ApertmantXamarin.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ApertmantXamarin
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(NewProperty), typeof(NewProperty));
            Routing.RegisterRoute(nameof(PropertyList), typeof(PropertyList));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(FilterPage), typeof(FilterPage));
        }

    }
}
