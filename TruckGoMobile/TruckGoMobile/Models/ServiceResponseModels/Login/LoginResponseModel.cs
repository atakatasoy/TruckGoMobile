using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class LoginResponseModel : BaseResponseModel
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string NameSurname { get; set; }
        public int UserType { get; set; }
    }
}
