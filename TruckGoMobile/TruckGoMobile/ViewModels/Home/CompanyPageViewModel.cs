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
        public ObservableCollection<SignalRUser> MessageList { get; set; } = new ObservableCollection<SignalRUser>();

        public delegate void NewMessageDelegate();
        public event NewMessageDelegate NewMessage;

        public ICommand SendCommand { get; set; }
        public ICommand RecordCommand { get; set; }
        public bool Recording { get; set; }
        public SignalRClient Client { get; private set; } = new SignalRClient();
        public string _username;
        public string MessageText { get; set; }
        AudioRecorderService recorder = new AudioRecorderService()
        {
            StopRecordingAfterTimeout = false,
            StopRecordingOnSilence = false,
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/XD.wav"
        };

        public CompanyPageViewModel()
        {
            RegisterWebServiceMethod(WaitTillConnectionEstablished);
            RegisterWebServiceMethod(FetchRecentMessages);

            _username = UserManager.Instance.CurrentLoggedInUser.UserNameSurname;

            Client.OnMessageReceived += AddMessage;

            SendCommand = new Command(() =>
            {
                Client.SendMessage(_username, MessageText, false);
                MessageText = string.Empty;
            });

            RecordCommand = new Command(async() => 
            {
                if (!recorder.IsRecording)
                {
                    await recorder.StartRecording();
                    Recording = true;
                }
                else
                {
                    await recorder.StopRecording();
                    Recording = false;

                    var filePath = recorder.GetAudioFilePath();

                    if (string.IsNullOrWhiteSpace(filePath))
                        return;

                    var bytes = File.ReadAllBytes(filePath);
                    var base64String = Convert.ToBase64String(bytes);

                    DialogManager.Instance.ShowIndicator("Bekle");

                    var response = await Helper.ApiCall<RegisterSoundResponseModel>(RequestType.Post, ControllerType.User, "registersound", JsonConvert.SerializeObject(new
                    {
                        UserManager.Instance.CurrentLoggedInUser.AccessToken,
                        SoundBase64String = base64String
                    }));

                    DialogManager.Instance.HideIndicator();
                    
                    if (response.responseVal == 0)
                    {
                        File.Move(filePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + response.fileId + ".wav");
                        Client.SendMessage(_username, response.fileId, true);
                    }
                    else
                        DialogManager.Instance.ShowDialog("Birşeyler ters gitti.Lütfen daha sonra tekrar deneyiniz");
                }
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

        async Task SetSoundPath(List<SignalRUser> list)
        {
            var fileIds = list.Select(u => u.Message);

            var response = await Helper.ApiCall<GetSoundsResponseModel>(RequestType.Post, ControllerType.User, "getsounds", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken,
                fileIds = new List<string>(fileIds)
            }));
            foreach (var sounds in response.soundsBase64Dic)
            {
                var bytes = Convert.FromBase64String(sounds.Value);
                var path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +"/"+ sounds.Key + ".wav";
                File.WriteAllBytes(path, bytes);
                list.FirstOrDefault(u => u.Message == sounds.Key).SavedSoundLocation = path;
            };
        }

        async Task SetSoundPath(SignalRUser user)
        {
            var response = await Helper.ApiCall<GetSoundsResponseModel>(RequestType.Post, ControllerType.User, "getsounds", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken,
                fileIds = new List<string>() { user.Message }
            }));
            foreach (var sounds in response.soundsBase64Dic)
            {
                var bytes = Convert.FromBase64String(sounds.Value);
                var path = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/" + sounds.Key + ".wav";
                File.WriteAllBytes(path, bytes);
                user.SavedSoundLocation = path;
            };
        }

        public void AddMessage(SignalRUser user)
        {
            MessageList.Add(user);
            NewMessage?.Invoke();
            if (user.IsSound)
            {
                if (user.Username != UserManager.Instance.CurrentLoggedInUser.UserNameSurname)
                    Task.Run(() => SetSoundPath(user));
                else
                    user.SavedSoundLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/" + user.Message + ".wav";
            }
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
                foreach(var message in response.messagesList)
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
                    Task.Run(() => SetSoundPath(listOfSounds));
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
