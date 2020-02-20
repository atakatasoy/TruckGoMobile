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
	public partial class CompanyPage : AnalyticsBasePage
	{   
        CompanyPageViewModel viewModel;
        public CompanyPage() : base(nameof(CompanyPage))
        {
            InitializeComponent();
            SetInteractionalComponents(sendButton);
            BindingContext = viewModel = new CompanyPageViewModel();
            messageEntry.ReturnType = ReturnType.Send;
            messageEntry.ReturnCommand = new Command(() => viewModel.SendCommand.Execute(null));
            viewModel.Client.ConnectionError += ShowError;
            viewModel.NewMessage += FocusToLastItem;
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

        private void MainList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (SignalRUser)e.Item;
            if (item.SavedSoundLocation != null) item.Play.Execute(null);
        }
    }
}