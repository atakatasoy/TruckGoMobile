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
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using TruckGoMobile.Droid.LocationService;
using TruckGoMobile.Interfaces;
using static Android.Net.ConnectivityManager;

[assembly: Xamarin.Forms.Dependency(typeof(LocationServiceManager))]
namespace TruckGoMobile.Droid.LocationService
{
    public class LocationServiceManager:ILocator
    {
        public static bool ServiceBinded { get; set; }

        MainActivity Activity = (MainActivity)CrossCurrentActivity.Current.Activity;

        public static LocationUpdatesService mInstance;

        public void StartLocationRequest()
        {
            if (!ServiceBinded)
            {
                BindService();
            }
            if (!Utils.RequestingLocationUpdates(Activity))
            {
                mInstance.RequestLocationUpdates();
            }
        }
        public void StopLocationRequest()
        {
            if (ServiceBinded)
            {
                if (Utils.RequestingLocationUpdates(Activity))
                {
                    mInstance.RemoveLocationUpdates();
                }
            }
        }
        public void UnBindService()
        {
            StopLocationRequest();
            Utils.SetServiceBinded(Activity, false);
            //Utils.SetServiceBinded(Activity, false);
        }
        public bool BindService()
        {
            if (!ServiceBinded)
            {
                Activity.ServiceConnection = new CustomServiceConnection() { Activity = Activity };

                ServiceBinded = Activity.BindService(new Intent(Activity, typeof(LocationUpdatesService)), Activity.ServiceConnection, Bind.AutoCreate);

                LocalBroadcastManager.GetInstance(Activity).RegisterReceiver(Activity.myReceiver,
                      new IntentFilter(LocationUpdatesService.ActionBroadcast));

                Activity.RegisterReceiver(Activity.mGpsStateListener, new IntentFilter(LocationManager.ProvidersChangedAction));
            }

            return ServiceBinded;
        }

        public LocationServiceManager()
        {
            var Activity = (MainActivity)CrossCurrentActivity.Current.Activity;
            //networkCall = new Monitor();
        }
        public void DisableListener()
        {
            //((ConnectivityManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.ConnectivityService)).UnregisterNetworkCallback(networkCall);
        }
    }
    public class Monitor : NetworkCallback
    {
        public Monitor()
        {
            ((ConnectivityManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.ConnectivityService))
                .RegisterNetworkCallback(new NetworkRequest.Builder().AddTransportType(TransportType.Wifi).AddTransportType(TransportType.Cellular).Build(), this);
        }
        //public override void OnAvailable(Network network)
        //{
        //    Utility.ShowProvider = false;
        //}
        //public override void OnUnavailable()
        //{
        //    Utility.ShowProvider = true;
        //}
        //public override void OnLost(Network network)
        //{
        //    Utility.ShowProvider = true;
        //}
    }
}
