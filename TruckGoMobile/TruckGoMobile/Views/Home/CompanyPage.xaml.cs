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
            BindingContext = viewModel = new CompanyPageViewModel();
            InitializeComponent();
            SetInteractionalComponents(sendButton);
            messageEntry.ReturnType = ReturnType.Send;
            messageEntry.ReturnCommand = new Command(() =>
            {
                viewModel.SendCommand.Execute(null);
            });
            viewModel.Client.ConnectionError += ShowError;
            viewModel.NewMessage += FocusToLastItem;
        }

        void FocusToLastItem()
        {
            Device.BeginInvokeOnMainThread(() => mainList.ScrollTo(viewModel.MessageList[viewModel.MessageList.Count - 1], ScrollToPosition.End, false));
        }

        async void ShowError()
        {
            await DisplayAlert("TruckGo", "Bağlantı sırasında hata oluştu.", "Tamam");
            await Navigation.PopAsync();
        }
        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            if (VoiceManager.Instance.IsRecording)
                await VoiceManager.Instance.StopRecording();
            VoiceManager.Instance.Pause();
            viewModel.NewMessage -= FocusToLastItem;
            viewModel.Client.ConnectionError -= ShowError;
            viewModel.Client.OnMessageReceived -= viewModel.AddMessage;
            VoiceManager.Instance.AddOrRemoveFinishedPlaying(viewModel.Toggle, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VoiceManager.Instance.AddOrRemoveFinishedPlaying(viewModel.Toggle, true);
        }

        bool recording = false;
        private void RecordAnimation_OnClick(object sender, EventArgs e)
        {
            var view = (Lottie.Forms.AnimationView)sender;
            if (!recording)
            {
                recording = true;
                view.Play();
                viewModel.RecordCommand.Execute(null);
            }
            else
            {
                recording = false;
                view.PlayProgressSegment(1f, 0f);
                viewModel.RecordCommand.Execute(null);
            }
        }
    }
}