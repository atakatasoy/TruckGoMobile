using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using TruckGoMobile.Droid.LocationService;
using Android.Content;
using Xamarin.Forms;
using TruckGoMobile.Interfaces;
using Android.Support.V4.Content;
using Android.Preferences;
using Plugin.Permissions;
using Rg.Plugins.Popup.Services;

namespace TruckGoMobile.Droid
{
    [Activity(Label = "TruckGoMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        #region Private Members
        const string Tag = "MainActivity";
        #endregion

        #region Public Members
        
        public LocationReceiver myReceiver;
        
        public LocationUpdatesService Service;

        public LocationProviderStateListener mGpsStateListener;

        public bool Bound;

        public CustomServiceConnection ServiceConnection;
        #endregion

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences,string key)
        {
            if (key.Equals(Utils.KeyServiceBinded))
            {
                if (!Utils.GetServiceBinded(this))
                {
                    if (Bound)
                    {
                        var service = DependencyService.Get<ILocator>();
                        service.StopLocationRequest();
                        UnbindService(ServiceConnection);
                        Bound = false;
                        LocationServiceManager.ServiceBinded = false;
                        LocalBroadcastManager.GetInstance(this).UnregisterReceiver(myReceiver);
                        UnregisterReceiver(mGpsStateListener);
                        PreferenceManager.GetDefaultSharedPreferences(this)
                            .UnregisterOnSharedPreferenceChangeListener(this);
                    }
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            ImageCircleRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            myReceiver = new LocationReceiver() { Context = this };
            DependencyService.Get<ILocator>();
            ServiceConnection = new CustomServiceConnection() { Activity = this };
            mGpsStateListener = new LocationProviderStateListener();
            LoadApplication(new App());
        }
        protected override void OnStart()
        {
            base.OnStart();
            PreferenceManager.GetDefaultSharedPreferences(this).RegisterOnSharedPreferenceChangeListener(this);
        }

        protected override void OnStop()
        {
            PreferenceManager.GetDefaultSharedPreferences(this).UnregisterOnSharedPreferenceChangeListener(this);
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            DependencyService.Get<ILocator>().DisableListener();
            DependencyService.Get<ILocator>().UnBindService();
            LocationServiceManager.mInstance = null;
            Finish();
            base.OnDestroy();
        }
    }
}