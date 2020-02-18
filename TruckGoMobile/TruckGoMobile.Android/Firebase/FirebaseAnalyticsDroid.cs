using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Analytics;
using Plugin.CurrentActivity;
using TruckGoMobile.Droid.Firebase;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAnalyticsDroid))]
namespace TruckGoMobile.Droid.Firebase
{
    public class FirebaseAnalyticsDroid : IFirebaseAnalytics
    {
        private string FixEventId(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return "unknown";

            //remove unwanted characters
            eventId = Regex.Replace(eventId, @"[^a-zA-Z0-9_]+", "_", RegexOptions.Compiled);

            //trim to 40 if needed
            return eventId.Substring(0, Math.Min(40, eventId.Length));
        }

        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            }); ;
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            eventId = FixEventId(eventId);

            var firebaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.Activity);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
        }
    }
}