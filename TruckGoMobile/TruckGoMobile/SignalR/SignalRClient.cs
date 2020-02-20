using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Services;

namespace TruckGoMobile.SignalR
{
    public class SignalRClient
    {
        string url = $"{Utility.BaseURL}";
        HubConnection Connection;
        IHubProxy ChatHubProxy;

        public delegate void Error();
        public delegate void MessageReceived(SignalRUser user);

        public event Error ConnectionError;
        public event MessageReceived OnMessageReceived;
        public bool? connectionEstablished = null;

        public void Connect()
        {
            var mUser = UserManager.Instance.CurrentLoggedInUser;

            Connection = new HubConnection(url);

            Connection.Headers.Add(new KeyValuePair<string, string>("AccessToken", mUser.AccessToken));
            Connection.Headers.Add(new KeyValuePair<string, string>("username", mUser.Username));

            ChatHubProxy = Connection.CreateHubProxy("ChatHub");
            ChatHubProxy.On<string, string,bool>("MessageReceived",
                (username, message,isSound) =>
                {
                    var user = new SignalRUser
                    {
                        Username = username,
                        Message = message,
                        IsSound = isSound
                    };
                    OnMessageReceived?.Invoke(user);
                });
            Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    ConnectionError?.Invoke();
                    connectionEstablished = false;
                }
                else
                    connectionEstablished = true;
            });
        }

        public void SendMessage(string username,string message,bool isSound)
        {
            ChatHubProxy.Invoke("SendMessage", username, message, isSound);
        }

        private Task Start()
        {
            return Connection.Start();
        }
    }
}
