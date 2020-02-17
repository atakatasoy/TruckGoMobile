using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TruckGoMobile.Models;
using TruckGoMobile.Services;
using TruckGoMobile.Views;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        MainPage RootPage => Application.Current.MainPage as MainPage;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(Login);
            if(UserManager.Instance.GetLastActiveUser() is User lastActiveUser)
            {
                Username = lastActiveUser.Username;
            }
        }

        public async void Login()
        {
            using (UserDialogs.Instance.Loading("Lütfen Bekleyiniz..."))
            {
                var response = await Helper.ApiCall<LoginResponseModel>(RequestType.Post, ControllerType.User, "login", JsonConvert.SerializeObject(
                    new
                    {
                        Username,
                        Password
                    }));

                if (response.responseVal == 0)
                {
                    Password = "";
                    var user = UserManager.CreateUserFromServiceResponse(response);
                    UserManager.Instance.LogInUser(user);

                    RootPage.IsGestureEnabled = true;
                    await (Application.Current.MainPage as MainPage).NavigateFromMenu((int)MenuItemType.Home);
                }
                else
                {
                    DialogManager.Instance.ShowDialog(response.responseText);
                }
            }
        }
    }
}
