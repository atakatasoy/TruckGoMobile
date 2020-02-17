using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TruckGoMobile.Views;

namespace TruckGoMobile.Droid.LocationService
{
    public class CustomServiceConnection : Java.Lang.Object, IServiceConnection
    {
        MainPage RootPage { get => Xamarin.Forms.Application.Current.MainPage as MainPage; }
        public MainActivity Activity { get; set; }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            LocationUpdateServiceBinder binder = (LocationUpdateServiceBinder)service;
            Activity.Service = LocationServiceManager.mInstance = binder.GetLocationUpdatesService();
            Activity.Bound = true;
            Utils.SetServiceBinded(Activity, true);
            Utils.SetRequestingLocationUpdates(Activity, false);
            RootPage.LocationServiceInitialized = true;
        }
        public void OnServiceDisconnected(ComponentName name)
        {
            Activity.Service = null;
            Activity.Bound = false;
        }
    }
}