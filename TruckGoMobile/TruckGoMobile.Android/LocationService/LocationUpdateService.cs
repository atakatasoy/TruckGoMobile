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
using Android.Locations;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Java.Lang;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Plugin.CurrentActivity;

namespace TruckGoMobile.Droid.LocationService
{
    /**
	 * A bound and started service that is promoted to a foreground service when location updates have
	 * been requested and all clients unbind.
	 *
	 * For apps running in the background on "O" devices, location is computed only once every 10
	 * minutes and delivered batched every 30 minutes. This restriction applies even to apps
	 * targeting "N" or lower which are run on "O" devices.
	 *
	 * This sample show how to use a long-running service for location updates. When an activity is
	 * bound to this service, frequent location updates are permitted. When the activity is removed
	 * from the foreground, the service promotes itself to a foreground service, and location updates
	 * continue. When the activity comes back to the foreground, the foreground service stops, and the
	 * notification assocaited with that service is removed.
	 */
    [Service(Label = "LocationUpdatesService", Enabled = true, Exported = true)]
    [IntentFilter(new string[] { "com.xamarin.TruckGoMobile.Droid.Location.LocationUpdatesService" })]
    public class LocationUpdatesService : Service
    {
        #region Private Members
        const string ExtraStartedFromNotification = LocationPackageName + ".started_from_notification";

        const string LocationPackageName = "com.xamarin.LocationUpdatesService";

        string ChannelId = "channel_01";

        IBinder Binder;

        MainActivity Activity { get; set; } = (MainActivity)CrossCurrentActivity.Current.Activity;

        /**
	     * The desired interval for location updates. Inexact. Updates may be more or less frequent.
	     */
        const long UpdateIntervalInMilliseconds = 30000;

        /**
		 * The fastest rate for active location updates. Updates will never be more frequent
		 * than this value.
		 */
        const long FastestUpdateIntervalInMilliseconds = UpdateIntervalInMilliseconds / 2;

        /**
		 * The identifier for the notification displayed for the foreground service.
		 */
        const int NotificationId = 12345678;

        /**
		 * Used to check whether the bound activity has really gone away and not unbound as part of an
		 * orientation change. We create a foreground service notification only if the former takes
		 * place.
		 */

        NotificationManager NotificationManager;

        /**
		 * Contains parameters used by {@link com.google.android.gms.location.FusedLocationProviderApi}.
		 */
        LocationRequest LocationRequest;

        /**
		 * Provides access to the Fused Location Provider API.
		 */
        FusedLocationProviderClient FusedLocationClient;
        /**
		 * Callback for changes in location.
		 */
        LocationCallback LocationCallback;

        Handler ServiceHandler;
        #endregion

        #region Public Members

        public string Tag = "LocationUpdatesService";

        public const string ActionBroadcast = LocationPackageName + ".broadcast";

        public const string ExtraLocation = LocationPackageName + ".location";

        /**
       * The current location.
       */
        public Location Location;
        #endregion

        #region Ctor
        public LocationUpdatesService()
        {
            Binder = new LocationUpdateServiceBinder(this);
        }
        #endregion

        #region Service Event Methods
        public override void OnCreate()
        {
            FusedLocationClient = Android.Gms.Location.LocationServices.GetFusedLocationProviderClient(this);

            LocationCallback = new LocationCallbackImpl { Service = this };

            CreateLocationRequest();
            GetLastLocation();

            HandlerThread handlerThread = new HandlerThread(Tag);
            handlerThread.Start();
            ServiceHandler = new Handler(handlerThread.Looper);

            NotificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                string name = GetString(Resource.String.app_name);
                NotificationChannel mChannel = new NotificationChannel(ChannelId, name, NotificationImportance.Default);
                NotificationManager.CreateNotificationChannel(mChannel);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //var startedFromNotification = intent.GetBooleanExtra(ExtraStartedFromNotification, false);

            // We got here because the user decided to remove location updates from the notification.
            //if (startedFromNotification)
            //{
            //    RemoveLocationUpdates();
            //    StopSelf();
            //}
            // Tells the system to not try to recreate the service after it has been killed.
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // Called when a client (MainActivity in case of this sample) comes to the foreground
            // and binds with this service. The service should cease to be a foreground service
            // when that happens.
            StartForeground(NotificationId, GetNotification());
            return Binder;
        }

        public override void OnRebind(Intent intent)
        {
            // Called when a client (MainActivity in case of this sample) returns to the foreground
            // and binds once again with this service. The service should cease to be a foreground
            // service when that happens.
            //StopForeground(true);
            //StartForeground(NotificationId, GetNotification());
            base.OnRebind(intent);
        }

        public override bool OnUnbind(Intent intent)
        {
            StopForeground(true);
            // Called when the last client (MainActivity in case of this sample) unbinds from this
            // service. If this method is called due to a configuration change in MainActivity, we
            // do nothing. Otherwise, we make this service a foreground service.
            //        if (!ChangingConfiguration && Utils.RequestingLocationUpdates(this))
            //        {
            //            Log.Info(Tag, "Starting foreground service");
            //            /*
            //// TODO(developer). If targeting O, use the following code.
            //if (Build.VERSION.SDK_INT == Build.VERSION_CODES.O) {
            //	mNotificationManager.startServiceInForeground(new Intent(this,
            //			LocationUpdatesService.class), NOTIFICATION_ID, getNotification());
            //} else {
            //	startForeground(NOTIFICATION_ID, getNotification());
            //}
            // */
            //            //StartForeground(NotificationId, GetNotification());
            //        }
            return true; // Ensures onRebind() is called when a client re-binds.
        }

        public override void OnDestroy()
        {
            ServiceHandler.RemoveCallbacksAndMessages(this.Class);
            base.OnDestroy();
        }

        public void OnNewLocation(Location location)
        {
            Location = location;

            Intent intent = new Intent(ActionBroadcast);

            intent.PutExtra(ExtraLocation, location);

            LocalBroadcastManager.GetInstance(ApplicationContext).SendBroadcast(intent);
        }
        #endregion

        #region Service Methods
        public void RequestLocationUpdates()
        {
            Utils.SetRequestingLocationUpdates(Activity, true);
            StartService(new Intent(ApplicationContext, typeof(LocationUpdatesService)));
            try
            {
                FusedLocationClient.RequestLocationUpdates(this.LocationRequest, this.LocationCallback, Looper.MyLooper());
            }
            catch (SecurityException unlikely)
            {
                Utils.SetRequestingLocationUpdates(Activity, false);
            }
        }

        public void RemoveLocationUpdates()
        {
            try
            {
                FusedLocationClient.RemoveLocationUpdates(LocationCallback);
                Utils.SetRequestingLocationUpdates(Activity, false);
                StopSelf();
            }
            catch (SecurityException unlikely)
            {
                Utils.SetRequestingLocationUpdates(Activity, true);
            }
        }
        
        Notification GetNotification()
        {
            Intent intent = new Intent(this, typeof(LocationUpdatesService));

            var text = Utils.GetLocationText(Location);

            intent.PutExtra(ExtraStartedFromNotification, true);

            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetContentTitle(Utils.GetLocationTitle(this))
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.icon)//change icon
                .SetWhen(JavaSystem.CurrentTimeMillis());

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                builder.SetChannelId(ChannelId);
            }

            return builder.Build();
        }

        private void GetLastLocation()
        {
            try
            {
                FusedLocationClient.LastLocation.AddOnCompleteListener(new GetLastLocationOnCompleteListener { Service = this });
            }
            catch (SecurityException unlikely)
            {
            }
        }

        void CreateLocationRequest()
        {
            LocationRequest = new LocationRequest();
            LocationRequest.SetInterval(UpdateIntervalInMilliseconds);
            LocationRequest.SetFastestInterval(FastestUpdateIntervalInMilliseconds);
            LocationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
        }
    }
    #endregion

    #region Helper Classes
    public class GetLastLocationOnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        public LocationUpdatesService Service { get; set; }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful && task.Result != null)
            {
                Service.Location = (Location)task.Result;
            }
            else
            {
            }
        }
    }

    class LocationCallbackImpl : LocationCallback
    {
        public LocationUpdatesService Service { get; set; }
        public override void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);
            Service.OnNewLocation(result.LastLocation);
        }
    }
    #endregion
}