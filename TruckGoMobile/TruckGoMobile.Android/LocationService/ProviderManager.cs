using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;

namespace TruckGoMobile.Droid.LocationService
{
    public enum PermissionType
    {
        GPS,
        Network
    }
    public class ProviderManager
    {
        public static bool ProviderEnabled(PermissionType type)
        {
            switch (type)
            {
                case PermissionType.GPS:
                    using (LocationManager lm = (LocationManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.LocationService))
                    {
                        return (lm.IsProviderEnabled(LocationManager.GpsProvider) || lm.IsProviderEnabled(LocationManager.NetworkProvider));
                    }

                case PermissionType.Network:
                    using (ConnectivityManager cm = (ConnectivityManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.ConnectivityService))
                    {
                        using (NetworkInfo info = cm.ActiveNetworkInfo)
                        {
                            return (info != null) ? info.IsConnected : false;
                        }
                    }

                default:
                    return false;
            }

        }
    }
}