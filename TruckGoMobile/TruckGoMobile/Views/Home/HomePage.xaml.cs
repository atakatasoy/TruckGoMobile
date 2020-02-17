using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TruckGoMobile.Models;
using TruckGoMobile.Views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        MainPage RootPage => Application.Current.MainPage as MainPage;

        HomePageViewModel viewModel;
		public HomePage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new HomePageViewModel();
            NavigationPage.SetHasNavigationBar(this, true);
            companyButton.Name = nameof(companyButton);
            emergencyButton.Name = nameof(emergencyButton);
            profileButton.Name = nameof(profileButton);
            friendsButton.Name = nameof(friendsButton);
            routeButton.Name = nameof(routeButton);
            obidiButton.Name = nameof(obidiButton);
            cameraButton.Name = nameof(cameraButton);
            weatherButton.Name = nameof(weatherButton);
        }
        private async void ImageButton_Clicked(object sender,ButtonEventArgs e)
        {
            Page requestedPage = null;
            switch (e.Name)
            {
                case nameof(companyButton):
                    requestedPage = new CompanyPage();
                    break;
                case nameof(emergencyButton):
                    requestedPage = new EmergencyPage();
                    break;
                case nameof(profileButton):
                    requestedPage = new ProfilePage();
                    break;
                case nameof(friendsButton):
                    requestedPage = new FriendsPage();
                    break;
                case nameof(routeButton):
                    requestedPage = new RoutePage();
                    break;
                case nameof(obidiButton):
                    requestedPage = new ObidiPage();
                    break;
                case nameof(cameraButton):
                    requestedPage = new CameraPage();
                    break;
                case nameof(weatherButton):
                    requestedPage = new WeatherPage();
                    break;
            }
            
            await Navigation.PushAsync(requestedPage, true);
        }
    }
}