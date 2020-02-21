using Newtonsoft.Json;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Services;

namespace TruckGoMobile
{
    public class VoiceManager
    {
        public static VoiceManager Instance { get; private set; }

        public bool IsRecording => recorder.IsRecording;

        AudioRecorderService recorder;
        AudioPlayer player;

        public string currentAudio;
        public static void Init()
        {
            Instance = new VoiceManager
            {
                recorder = new AudioRecorderService()
                {
                    StopRecordingAfterTimeout = false,
                    StopRecordingOnSilence = false,
                    FilePath = Helper.CreateDirectoryForSoundFile("temp"),
                },
                player = new AudioPlayer()
            };
            Instance.player.FinishedPlaying += (sender, e) => SignalRUser.CurrentPlaying = null;
        }

        public void AddOrRemoveFinishedPlaying(Action<object,EventArgs> method,bool add)
        {
            var eventHandler = new EventHandler(method);
            if (add)
                player.FinishedPlaying += eventHandler;
            else
                player.FinishedPlaying -= eventHandler;
        }

        #region Player Methods
        public void Play(string filePath)
        {
            player.Play(filePath);
            currentAudio = filePath;
        }
        public void Pause()
        {
            player.Pause();
        }
        #endregion

        #region Recorder Methods

        public async Task StartRecording()
        {
            await recorder.StartRecording();
        }

        public async Task StopRecording()
        {
            await recorder.StopRecording();
        }

        public string GetRecordedFilePath()
        {
            return recorder.GetAudioFilePath();
        }

        public async Task<GetSoundsResponseModel> GetRoomSoundsFromService(IEnumerable<string> list)
        {
            return await Helper.ApiCall<GetSoundsResponseModel>(RequestType.Post, ControllerType.User, "getsounds", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken,
                fileIds = new List<string>(list)
            }));
        }

        public string CreateSoundFile(string fileId,string bytes)
        {
            var bufferBytes = Convert.FromBase64String(bytes);
            var path = Helper.CreateDirectoryForSoundFile(fileId);
            File.WriteAllBytes(path, bufferBytes);
            return path;
        }

        public async void LoadSounds(IEnumerable<SignalRUser> list)
        {
            if (list.Count() == 0)
                return;

            var fileIds = list.Select(u => u.Message);

            var response = await GetRoomSoundsFromService(fileIds);

            foreach (var sounds in response.soundsBase64Dic)
            {
                list.FirstOrDefault(u => u.Message == sounds.Key).SavedSoundLocation = CreateSoundFile(sounds.Key, sounds.Value);
            };
        }

        public async void LoadSound(SignalRUser user)
        {
            if (user == null)
                return;

            GetSoundsResponseModel response = await GetRoomSoundsFromService(user.Message);

            foreach (var sounds in response.soundsBase64Dic)
            {
                user.SavedSoundLocation = CreateSoundFile(sounds.Key, sounds.Value);
            };
        }

        public async Task<GetSoundsResponseModel> GetRoomSoundsFromService(string single)
        {
            return await GetRoomSoundsFromService(new List<string> { single });
        }

        public async Task<RegisterSoundResponseModel> SendFileToService(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            var base64String = Convert.ToBase64String(bytes);

            DialogManager.Instance.ShowIndicator("Bekle");

            var response = await Helper.ApiCall<RegisterSoundResponseModel>(RequestType.Post, ControllerType.User, "registersound", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken,
                SoundBase64String = base64String
            }));

            DialogManager.Instance.HideIndicator();

            return response;
        }

        #endregion
    }
}
