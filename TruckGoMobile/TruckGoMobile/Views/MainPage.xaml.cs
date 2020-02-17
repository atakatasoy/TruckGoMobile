using TruckGoMobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TruckGoMobile.Services;
using TruckGoMobile.Interfaces;
using TruckGoMobile.Views;
using TruckGoMobile.Views.Home;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Linq;

namespace TruckGoMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        bool mLocationServiceInitialized = false;
        public bool LocationServiceInitialized
        {
            get => mLocationServiceInitialized;
            set
            {
                if (value)
                    DependencyService.Get<ILocator>().StartLocationRequest();
                
                mLocationServiceInitialized = value;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;

            if (UserManager.Instance.LogInActiveUser())
            {
                var homePageKey = (int)MenuItemType.Home;
                MenuPages.Add(homePageKey, new NavigationPage(new HomePage()));
                Detail = MenuPages[homePageKey];
                IsGestureEnabled = true;
            }
            else
                IsGestureEnabled = false;

            var staticClassInitiation = Utility.BaseURL;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                A:
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    (await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location)).Select(perm => perm.Value).ToList().ForEach(permStatus => 
                    {
                        if (permStatus != PermissionStatus.Granted)
                        {
                            status = PermissionStatus.Denied;
                        }
                    });
                }
                if (status != PermissionStatus.Granted)
                {
                    goto A;
                }
            }
            catch
            {

            }
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Login:
                        MenuPages.Add(id, new NavigationPage(new LoginPage()));
                        break;
                    case (int)MenuItemType.Camera:
                        MenuPages.Add(id, new NavigationPage(new CameraPage()));
                        break;
                    case (int)MenuItemType.Company:
                        MenuPages.Add(id, new NavigationPage(new CompanyPage()));
                        break;
                    case (int)MenuItemType.Emergency:
                        MenuPages.Add(id, new NavigationPage(new EmergencyPage()));
                        break;
                    case (int)MenuItemType.Friends:
                        MenuPages.Add(id, new NavigationPage(new FriendsPage()));
                        break;
                    case (int)MenuItemType.Obidi:
                        MenuPages.Add(id, new NavigationPage(new ObidiPage()));
                        break;
                    case (int)MenuItemType.Profile:
                        MenuPages.Add(id, new NavigationPage(new ProfilePage()));
                        break;
                    case (int)MenuItemType.Route:
                        MenuPages.Add(id, new NavigationPage(new RoutePage()));
                        break;
                    case (int)MenuItemType.Weather:
                        MenuPages.Add(id, new NavigationPage(new WeatherPage()));
                        break;
                }
            }

            if (id == (int)MenuItemType.Logout)
            {
                UserManager.Instance.LogOffUser();
                IsPresented = false;
                IsGestureEnabled = false;
                if (LocationServiceInitialized)
                {
                    DependencyService.Get<ILocator>().UnBindService();
                    LocationServiceInitialized = false;
                }
                if (!MenuPages.ContainsKey((int)MenuItemType.Login))
                    MenuPages.Add((int)MenuItemType.Login, new NavigationPage(new LoginPage()));
                Detail = MenuPages[(int)MenuItemType.Login];
                return;
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}