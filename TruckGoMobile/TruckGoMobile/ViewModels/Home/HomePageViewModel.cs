using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            DependencyService.Get<ILocator>().BindService();
            DependencyService.Get<IFirebaseAnalytics>().SendEvent("HomePageLoaded");
        }
    }
}
