using System;
using System.Collections.Generic;
using System.Text;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class UserManager
    {
        public static UserManager Instance = new UserManager();

        public User CurrentLoggedInUser { get; private set; }

        public static User CreateUserFromServiceResponse(LoginResponseModel response)
        {
            if (response.responseVal != 0)
                throw new InvalidOperationException("Service didnt return successful response");

            return new User()
            {
                AccessToken = response.AccessToken,
                Username = response.Username,
                UserNameSurname = response.NameSurname,
                LoggedIn = true,
                LastLoggedInDate = DateTime.Now
            };
        }

        public bool LogInActiveUser()
        {
            var lastLoggedInUser = FindActiveUser();

            if (lastLoggedInUser == null)
                return false;

            LogInUser(lastLoggedInUser);
            CurrentLoggedInUser = lastLoggedInUser;

            return true;
        }

        User FindActiveUser()
        {
            var loggedInUser = default(User);

            using (var con = DependencyService.Get<IDatabase>().GetConnection())
                loggedInUser = con.Table<User>().FirstOrDefault(user => user.LoggedIn);

            return loggedInUser;
        }

        public void LogInUser(User loginUser)
        {
            CurrentLoggedInUser = loginUser;

            using(var con = DependencyService.Get<IDatabase>().GetConnection())
            {
                if (con.Table<User>().FirstOrDefault(user => user.AccessToken == loginUser.AccessToken) is User registeredUser)
                {
                    if(!registeredUser.LoggedIn)
                        registeredUser.SetProperty(nameof(registeredUser.LoggedIn), true, con);
                    registeredUser.SetProperty(nameof(registeredUser.LastLoggedInDate), DateTime.Now, con);
                }
                else
                {
                    loginUser.LoggedIn = true;
                    loginUser.LastLoggedInDate = DateTime.Now;
                    con.Insert(loginUser);
                }
            }
        }

        public void LogOffUser()
        {
            if (CurrentLoggedInUser == null)
                throw new InvalidOperationException("There is no LoggedIn user currently");

            CurrentLoggedInUser.SetProperty(nameof(CurrentLoggedInUser.LoggedIn), false);
            CurrentLoggedInUser = null;
        }

        public User GetLastActiveUser()
        {
            var lastLoggedInUser = default(User);

            using (var con = DependencyService.Get<IDatabase>().GetConnection())
                lastLoggedInUser = con.Table<User>().OrderByDescending(user => user.LastLoggedInDate).FirstOrDefault();
            
            return lastLoggedInUser;
        }
    }
}
