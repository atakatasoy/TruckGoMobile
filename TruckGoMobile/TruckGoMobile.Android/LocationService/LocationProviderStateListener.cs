using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile.Droid.LocationService
{
    public class LocationProviderStateListener : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals(LocationManager.ProvidersChangedAction))
            {
                if (ProviderManager.ProviderEnabled(PermissionType.GPS))
                {
                    if (Utils.RequestingLocationUpdates(CrossCurrentActivity.Current.Activity))
                    {
                        Utils.SetRequestingLocationUpdates(CrossCurrentActivity.Current.Activity, false);
                        DependencyService.Get<ILocator>().StartLocationRequest();
                    }
                    DialogManager.Instance.HideIndicator();
                }//Show privder
                else
                {
                    DialogManager.Instance.ShowIndicator("Lütfen konum bilginizi açın");
                    DependencyService.Get<ILocator>().StopLocationRequest();
                }//Hide provider
            }
        }
    }
}