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
	public partial class FriendsPage : AnalyticsBasePage
	{
        FriendsPageViewModel viewModel;
        public FriendsPage() : base(nameof(FriendsPage))
        {
            InitializeComponent();
            BindingContext = viewModel = new FriendsPageViewModel();
        }
	}
}