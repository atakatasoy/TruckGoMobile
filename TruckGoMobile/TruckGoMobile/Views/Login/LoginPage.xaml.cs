using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        LoginPageViewModel viewModel;
		public LoginPage ()
		{
			InitializeComponent ();
            usernameEntry.Name = "Username";
            passwordEntry.Name = "Password";
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel = new LoginPageViewModel();
		}

        private void Entry_Returned(object sender, EntryEventArgs e)
        {
            if (e.Name == "Username")
                passwordEntry.FocusEntry();
            else
                viewModel.LoginCommand.Execute(null);
        }
    }
}