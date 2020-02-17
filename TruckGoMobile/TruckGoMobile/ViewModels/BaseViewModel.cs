using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Models;

namespace TruckGoMobile
{
    public class BaseViewModel : INotifyPropertyChanged, IServiceInfo
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        LoadingContentPage.WebServiceDataRetrieveHandlers mServiceMethods;

        public LoadingContentPage.WebServiceDataRetrieveHandlers GetServiceMethods()
        {
            return mServiceMethods;
        }

        public void RegisterWebServiceMethod(LoadingContentPage.WebServiceDataRetrieveHandlers t)
        {
            mServiceMethods += t;
        }
    }
}
