using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Models;
using TruckGoMobile.SignalR;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views.Home
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyPage : LoadingContentPage
	{   
        CompanyPageViewModel viewModel;
		public CompanyPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new CompanyPageViewModel();
            messageEntry.ReturnType = ReturnType.Send;
            messageEntry.ReturnCommand = new Command(() => viewModel.SendCommand.Execute(null));
            viewModel.Client.ConnectionError += ShowError;
            viewModel.NewMessage += FocusToLastItem;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void FocusToLastItem()
        {
            Device.BeginInvokeOnMainThread(() => mainList.ScrollTo(viewModel.MessageList[viewModel.MessageList.Count - 1], ScrollToPosition.End, true));
        }

        async void ShowError()
        {
            await DisplayAlert("TruckGo", "Bağlantı sırasında hata oluştu.", "Tamam");
            await Navigation.PopAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.NewMessage -= FocusToLastItem;
            viewModel.Client.ConnectionError -= ShowError;
            viewModel.Client.OnMessageReceived -= viewModel.AddMessage;
        }
    }
}