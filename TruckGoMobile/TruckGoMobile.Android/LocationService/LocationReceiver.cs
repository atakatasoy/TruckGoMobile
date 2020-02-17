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
using Newtonsoft.Json;
using TruckGoMobile.Services;

namespace TruckGoMobile.Droid.LocationService
{
    public class LocationReceiver : BroadcastReceiver
    {
        //Location json string , responseVal
        static Dictionary<string, int> _failedLocations = new Dictionary<string, int>();

        public Context Context { get; set; }

        public async override void OnReceive(Context context, Intent intent)
        {
            var location = intent.GetParcelableExtra(LocationUpdatesService.ExtraLocation) as Location;

            if (location != null)
            {
                var data = JsonConvert.SerializeObject(new
                {
                    UserManager.Instance.CurrentLoggedInUser.AccessToken,
                    Latitude = location.Latitude.ToString(),
                    Longitude = location.Longitude.ToString(),
                    Speed = (location.Speed * 1.609344).ToString(),
                    Altitude = location.Altitude.ToString(),
                    Accuracy = location.Accuracy.ToString(),
                    Date = DateTime.Now.ToString()
                });

                var response = await Helper.ApiCall<BaseResponseModel>(RequestType.Post, ControllerType.User, "setlocationupdates", data);

                if (response.responseVal == -3)
                    _failedLocations.Add(data);
            }
        }
    }
}