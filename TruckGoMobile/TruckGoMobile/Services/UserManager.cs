using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

            Task.Run(() => DependencyService.Get<IFirebaseAnalytics>().SendEvent("Login_WithActiveUser", new Dictionary<string, string>
            {
                {"NameSurname",CurrentLoggedInUser.UserNameSurname }
            }));

            LogInUser(lastLoggedInUser, false);

            return true;
        }

        User FindActiveUser()
        {
            var loggedInUser = default(User);

            using (var con = DependencyService.Get<IDatabase>().GetConnection())
                loggedInUser = con.Table<User>().FirstOrDefault(user => user.LoggedIn);

            return loggedInUser;
        }

        public void LogInUser(User loginUser, bool useAnalytic = true)
        {
            CurrentLoggedInUser = loginUser;

            using (var con = DependencyService.Get<IDatabase>().GetConnection())
            {
                if (con.Table<User>().FirstOrDefault(user => user.AccessToken == loginUser.AccessToken) is User registeredUser)
                {
                    if (!registeredUser.LoggedIn)
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
            if (useAnalytic)
                Task.Run(() => DependencyService.Get<IFirebaseAnalytics>().SendEvent("Login_WithPass", new Dictionary<string, string>
                {
                    {"NameSurname",CurrentLoggedInUser.UserNameSurname }
                }));
        }

        public void LogOffUser()
        {
            if (CurrentLoggedInUser == null)
                throw new InvalidOperationException("There is no LoggedIn user currently");

            Task.Run(() => DependencyService.Get<IFirebaseAnalytics>().SendEvent("LogOff", new Dictionary<string, string>
            {
                {"NameSurname",CurrentLoggedInUser.UserNameSurname }
            }));

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
