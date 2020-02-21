using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class SignalRUser : INotifyPropertyChanged
    {
        public static SignalRUser CurrentPlaying = null;
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public string Username { get; set; }
        public string Message { get; set; }
        public bool IsSound { get; set; }
        public string SavedSoundLocation { get; set; }
        public ICommand Play { get; set; }

        public ImageSource Image { get; set; }

        public bool IsPlaying = false;
        public SignalRUser()
        {
            Image = "../play.png";
            Play = new Command(() =>
            {
                if (!IsPlaying)
                {
                    CurrentPlaying?.SetImageToPlay();
                    CurrentPlaying = this;
                }
                    
                TogglePlaying();
            });
        }
        public void TogglePlaying()
        {
            if (!IsPlaying)
            {
                Image = "../pause.png";
                VoiceManager.Instance.Play(SavedSoundLocation);
            }
            else
            {
                Image = "../play.png";
                VoiceManager.Instance.Pause();
            }
            IsPlaying = IsPlaying ? false : true;
        }
        public void ToggleImage()
        {
            if (!IsPlaying)
                Image = "../pause.png";
            else
                Image = "../play.png";

            IsPlaying = !IsPlaying;
        }
        public void SetImageToPlay()
        {
            Image = "../play.png";
            IsPlaying = false;
        }
    }
}
