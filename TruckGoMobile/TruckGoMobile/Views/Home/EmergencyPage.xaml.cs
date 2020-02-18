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
	public partial class EmergencyPage : AnalyticsBasePage
	{
        EmergencyPageViewModel viewModel;
        public EmergencyPage() : base(nameof(EmergencyPage))
        {
            InitializeComponent();
            BindingContext = viewModel = new EmergencyPageViewModel();
            SetInteractionalComponents(bribeButton, stolenButton, accidentButton);
        }
	}
}