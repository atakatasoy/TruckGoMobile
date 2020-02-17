using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TruckGoMobile.Models;
using TruckGoMobile.Services;
using TruckGoMobile.SignalR;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class CompanyPageViewModel : BaseViewModel
    {
        public ObservableCollection<SignalRUser> MessageList { get; set; } = new ObservableCollection<SignalRUser>();

        public delegate void NewMessageDelegate();
        public event NewMessageDelegate NewMessage;

        public ICommand SendCommand { get; set; }
        public SignalRClient Client { get; private set; } = new SignalRClient();
        public string _username;
        public string MessageText { get; set; }
        
        public CompanyPageViewModel()
        {
            RegisterWebServiceMethod(WaitTillConnectionEstablished);
            RegisterWebServiceMethod(FetchRecentMessages);

            _username = UserManager.Instance.CurrentLoggedInUser.UserNameSurname;

            Client.OnMessageReceived += AddMessage;

            SendCommand = new Command(() =>
            {
                Client.SendMessage(_username, MessageText);
                MessageText = string.Empty;
            });
            Client.Connect();
        }

        public async Task WaitTillConnectionEstablished()
        {
            await Task.Run(() =>
            {
                while (Client.connectionEstablished == null) ;
            });
        }

        public void AddMessage(SignalRUser user)
        {
            MessageList.Add(user);
            NewMessage?.Invoke();
        }

        public async Task FetchRecentMessages()
        {
            MessageList = new ObservableCollection<SignalRUser>();

            var response = await Helper.ApiCall<GetUserMessagesResponseModel>(RequestType.Post, ControllerType.User, "getroommessages", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken
            }));

            if (response.responseVal == 0)
            {
                foreach(var message in response.messagesList)
                {
                    MessageList.Add(new SignalRUser
                    {
                        Username = message.MessageOwner,
                        Message = message.MessageContent
                    });
                }
                NewMessage?.Invoke();
            }
            else
            {
                MessageList.Add(new SignalRUser
                {
                    Username = "",
                    Message = response.responseText
                });
            }
        }
    }
}
