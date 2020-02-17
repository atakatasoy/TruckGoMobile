using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class GetUserInfoResponseModel : BaseResponseModel
    {
        public UserInfoModel userInfo { get; set; }
    }
}
