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
        static AudioPlayer player = new AudioPlayer();

        public string Username { get; set; }
        public string Message { get; set; }
        public bool IsSound { get; set; }
        public string SavedSoundLocation { get; set; }
        public ICommand Play { get; set; }

        public SignalRUser()
        {
            Play = new Command(() =>
            {
                if (SavedSoundLocation != null)
                    player.Play(SavedSoundLocation);
            });
        }
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}
