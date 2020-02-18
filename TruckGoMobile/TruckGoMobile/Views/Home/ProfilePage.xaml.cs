using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views.Home
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : AnalyticsBasePage
    {
        ProfilePageViewModel viewModel;
        public ProfilePage() : base(nameof(ProfilePage))
        {
            InitializeComponent();
            BindingContext = viewModel = new ProfilePageViewModel();
            AddInteractionalComponent(editButton);
        }
	}
}