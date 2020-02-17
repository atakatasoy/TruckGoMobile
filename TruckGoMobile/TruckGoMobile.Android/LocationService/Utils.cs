using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace TruckGoMobile.Droid.LocationService
{
    public class Utils
    {
        public const string KeyRequestingLocationUpdates = "requesting_locaction_updates";

        public const string KeyServiceBinded = "service_binded";

        public static bool GetServiceBinded(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context)
                .GetBoolean(KeyServiceBinded, false);
        }

        public static void SetServiceBinded(Context context, bool serviceBindedValue)
        {
            PreferenceManager.GetDefaultSharedPreferences(context)
                .Edit()
                .PutBoolean(KeyServiceBinded, serviceBindedValue)
                .Apply();
        }

        public static bool RequestingLocationUpdates(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context)
                    .GetBoolean(KeyRequestingLocationUpdates, false);
        }

        public static void SetRequestingLocationUpdates(Context context, bool requestingLocationUpdates)
        {
            PreferenceManager.GetDefaultSharedPreferences(context)
                    .Edit()
                    .PutBoolean(KeyRequestingLocationUpdates, requestingLocationUpdates)
                    .Apply();
        }

        public static string GetLocationText(Location location)
        {
            return location == null ? "Unknown location" :
                    "(" + location.Latitude + ", " + location.Longitude + ")";
        }

        public static string GetLocationTitle(Context context)
        {
            return context.GetString(Resource.String.location_updated,
                    DateFormat.GetDateFormat(context).Format(new Date()));
        }
    }
}