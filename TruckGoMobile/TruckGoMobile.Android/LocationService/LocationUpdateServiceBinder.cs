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

namespace TruckGoMobile.Droid.LocationService
{
    public class LocationUpdateServiceBinder : Binder
    {
        public readonly LocationUpdatesService service;

        public LocationUpdateServiceBinder(LocationUpdatesService service)
        {
            this.service = service;
        }

        public LocationUpdatesService GetLocationUpdatesService()
        {
            return service;
        }
    }
}