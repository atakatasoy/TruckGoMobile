using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class GetSoundsResponseModel : BaseResponseModel
    {
        public Dictionary<string, string> soundsBase64Dic { get; set; }
    }
}
