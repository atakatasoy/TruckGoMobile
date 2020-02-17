using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile
{
    public class GetUserMessagesResponseModel : BaseResponseModel
    {
        public List<MessageListModel> messagesList { get; set; }
    }
}
