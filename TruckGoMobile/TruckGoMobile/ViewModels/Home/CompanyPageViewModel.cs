using Newtonsoft.Json;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        #region Public Members
        public ObservableCollection<SignalRUser> MessageList { get; set; } = new ObservableCollection<SignalRUser>();

        public delegate void NewMessageDelegate();
        public event NewMessageDelegate NewMessage;

        public ICommand SendCommand { get; set; }
        public ICommand RecordCommand { get; set; }
        public bool Recording { get; set; }
        public SignalRClient Client { get; private set; } = new SignalRClient();
        public string _username;
        public string MessageText { get; set; } 
        #endregion

        public CompanyPageViewModel()
        {
            RegisterWebServiceMethod(WaitTillConnectionEstablished);
            RegisterWebServiceMethod(FetchRecentMessages);
            MessageText = "";

            _username = UserManager.Instance.CurrentLoggedInUser.UserNameSurname;

            Client.OnMessageReceived += AddMessage;

            SendCommand = new Command(() =>
            {
                Client.SendMessage(_username, MessageText, false);
                MessageText = string.Empty;
            });

            RecordCommand = new Command(RecordSound);

            Client.Connect();
        }

        #region Service Methods
        public async Task WaitTillConnectionEstablished()
        {
            await Task.Run(() =>
            {
                while (Client.connectionEstablished == null) ;
            });
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
                var listOfSounds = new List<SignalRUser>();
                foreach (var message in response.messagesList)
                {
                    var user = new SignalRUser
                    {
                        Username = message.MessageOwner,
                        Message = message.MessageContent,
                        IsSound = message.IsSound
                    };

                    MessageList.Add(user);

                    if (user.IsSound)
                    {
                        listOfSounds.Add(user);
                    }
                }
                if (listOfSounds.Count != 0)
                {
                    listOfSounds = listOfSounds.Where(sound =>
                    {
                        var path = Helper.CreateDirectoryForSoundFile(sound.Message);
                        if (!File.Exists(path))
                            return true;

                        sound.SavedSoundLocation = path;
                        return false;
                    }).ToList();

                    Task.Run(() => VoiceManager.Instance.LoadSounds(listOfSounds));
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
        #endregion

        #region Helper Methods
        async void RecordSound()
        {
            if (!VoiceManager.Instance.IsRecording)
            {
                await VoiceManager.Instance.StartRecording();
                Recording = true;
            }
            else
            {
                await VoiceManager.Instance.StopRecording();
                Recording = false;

                var filePath = VoiceManager.Instance.GetRecordedFilePath();

                if (string.IsNullOrWhiteSpace(filePath))
                    return;

                var response = await VoiceManager.Instance.SendFileToService(filePath);

                if (response.responseVal == 0)
                {
                    File.Move(filePath, Helper.CreateDirectoryForSoundFile(response.fileId));
                    Client.SendMessage(_username, response.fileId, true);
                }
                else
                    DialogManager.Instance.ShowDialog("Birşeyler ters gitti.Lütfen daha sonra tekrar deneyiniz");
            }
        }

        public void AddMessage(SignalRUser user)
        {
            MessageList.Add(user);
            NewMessage?.Invoke();
            if (user.IsSound)
            {
                if (user.Username != UserManager.Instance.CurrentLoggedInUser.UserNameSurname)
                    Task.Run(() => VoiceManager.Instance.LoadSound(user));
                else
                    user.SavedSoundLocation = Helper.CreateDirectoryForSoundFile(user.Message);
            }
        }

        public void Toggle(object sender, EventArgs e)
        {
            MessageList.Where(m => m.IsSound).FirstOrDefault(m => m.SavedSoundLocation == VoiceManager.Instance.currentAudio)?.ToggleImage();
        } 
        #endregion
    }
}
