using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        static List<Location> _failedLocations = new List<Location>();

        public Context Context { get; set; }
        
        object CreateLocationObject(Location location)
        {
            return new
            {
                Latitude = location.Latitude.ToString(),
                Longitude = location.Longitude.ToString(),
                Speed = (location.Speed * 1.609344).ToString(),
                Altitude = location.Altitude.ToString(),
                Accuracy = location.Accuracy.ToString(),
                Date = DateTime.Now.ToString()
            };
        }

        string GetLocationString(Location location)
        {
            var data = new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken,
                LocationList = new[]
                {
                    CreateLocationObject(location)
                }.ToList()
            };

            foreach(var eachLocation in _failedLocations)
            {
                dynamic jsonObject = CreateLocationObject(eachLocation);
                data.LocationList.Add(jsonObject);
            }

            return JsonConvert.SerializeObject(data);
        }

        async Task<BaseResponseModel> SendLocation(string serializedData)
        {
            return await Helper.ApiCall<BaseResponseModel>(RequestType.Post, ControllerType.User, "setlocationupdates", serializedData);
        }

        public async override void OnReceive(Context context, Intent intent)
        {
            var location = intent.GetParcelableExtra(LocationUpdatesService.ExtraLocation) as Location;
            
            if (location != null)
            {
                var data = GetLocationString(location);

                var response = await SendLocation(data);

                if (response.responseVal == -3)
                    _failedLocations.Add(location);
                else if (response.responseVal == 0)
                    _failedLocations.Clear();
                else
                    await Helper.LogError(response.responseText, DateTime.Now.ToString("dd.mm.yyyy HH:mm:ss"), data);
            }
        }
    }
}