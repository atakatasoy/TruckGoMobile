using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TruckGoMobile
{
    public class DialogManager
    {
        public static DialogManager Instance { get; } = new DialogManager();

        string CurrentDialogType { get; } = "Alert";
        string IndicatorTitleText { get; } = "Lütfen Bekleyiniz...";
        public bool IndicatorVisible { get; set; }

        public void ShowDialog(string message)
        {
            switch (CurrentDialogType)
            {
                case "Alert":
                    UserDialogs.Instance.Alert(message, "Uyarı", "Tamam");
                    break;
                case "Toast":
                    UserDialogs.Instance.Toast(message);
                    break;
            }
        }
        public async Task ShowIndicatorAsync()
        {
            if (IndicatorVisible)
                return;
            
            IndicatorVisible = true;
            UserDialogs.Instance.ShowLoading(IndicatorTitleText);

            //Releasing the UI Thread for indicator to be popped up
            await Task.Delay(100);
        }
        public void ShowIndicator(string message = null)
        {
            if (IndicatorVisible)
                return;

            IndicatorVisible = true;
            UserDialogs.Instance.ShowLoading(message != null ? message : IndicatorTitleText);
        }
        public void HideIndicator()
        {       
            if (!IndicatorVisible)
                return;

            IndicatorVisible = false;
            UserDialogs.Instance.HideLoading();
        }
    }
}
