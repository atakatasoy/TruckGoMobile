using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Models;
namespace TruckGoMobile
{
    public interface IServiceInfo
    {
        LoadingContentPage.WebServiceDataRetrieveHandlers GetServiceMethods();
        void RegisterWebServiceMethod(LoadingContentPage.WebServiceDataRetrieveHandlers t);
    }
}
