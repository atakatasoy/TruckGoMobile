using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TruckGoMobile.Models;
using TruckGoMobile.Views.Home;
using TruckGoMobile.Views.PartialView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : AnalyticsBasePage
	{
        MainPage RootPage => Application.Current.MainPage as MainPage;
        Dictionary<string, CircleButton> menuButtons;

        HomePageViewModel viewModel;
        public HomePage() : base(nameof(HomePage))
        {
            InitializeComponent();
            BindingContext = viewModel = new HomePageViewModel();
            NavigationPage.SetHasNavigationBar(this, true);

            menuButtons = new Dictionary<string, CircleButton>()
            {
                {nameof(companyButton),companyButton },
                {nameof(emergencyButton),emergencyButton },
                {nameof(profileButton),profileButton },
                {nameof(friendsButton),friendsButton },
                {nameof(routeButton),routeButton },
                {nameof(obidiButton) ,obidiButton},
                {nameof(cameraButton),cameraButton },
                {nameof(weatherButton),weatherButton }
            };

            foreach (var circleButton in menuButtons)
            {
                circleButton.Value.Name = circleButton.Key;
                AddInteractionalComponent(circleButton.Value);
            }
        }

        private async void ImageButton_Clicked(object sender,EventArgs e)
        {
            Page requestedPage = null;
            var isCircleButton = sender as CircleButton;
            switch (isCircleButton.Name)
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