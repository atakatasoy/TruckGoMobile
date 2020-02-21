using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class GetSoundsResponseModel : BaseResponseModel
    {
        //fileId,fileBytes
        public Dictionary<string, string> soundsBase64Dic { get; set; }
    }
}
