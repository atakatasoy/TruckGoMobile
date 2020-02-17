using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Services;

namespace TruckGoMobile
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public UserInfoModel UserInfo { get; set; }

        public ProfilePageViewModel()
        {
            RegisterWebServiceMethod(SetProfileInfo);
        }

        public async Task SetProfileInfo()
        {
            var response = await Helper.ApiCall<GetUserInfoResponseModel>(RequestType.Post, ControllerType.User, "getprofileinfo", JsonConvert.SerializeObject(new
            {
                UserManager.Instance.CurrentLoggedInUser.AccessToken
            }));

            if (response.responseVal == 0)
            {
                UserInfo = response.userInfo;
            }
            else
            {
                DialogManager.Instance.ShowDialog(response.responseText);
            }
        }
    }
}
