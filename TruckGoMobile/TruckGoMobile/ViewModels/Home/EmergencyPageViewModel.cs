using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TruckGoMobile.Services;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class EmergencyPageViewModel : BaseViewModel
    {
        public ICommand EmergencyCommand { get; set; }
        public EmergencyPageViewModel()
        {
            EmergencyCommand = new Command(async(emergencyType) =>
            {
                await DialogManager.Instance.ShowIndicatorAsync();

                var response = await Helper.ApiCall<BaseResponseModel>(RequestType.Post, ControllerType.User, "emergencycall", JsonConvert.SerializeObject(new
                {
                    UserManager.Instance.CurrentLoggedInUser.AccessToken,
                    Emergency = (int)emergencyType
                }));

                DialogManager.Instance.HideIndicator();

                if (response.responseVal == 0)
                {
                    response.responseText = "İşlem Başarılı";
                }
                DialogManager.Instance.ShowDialog(response.responseText);
            });
        }
    }
}
