using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public HomePageViewModel()
        {
            DependencyService.Get<ILocator>().BindService();
        }
    }
}
