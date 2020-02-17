using System;
using System.Collections.Generic;
using System.Text;

namespace TruckGoMobile.Interfaces
{
    public interface ILocator
    {
        bool BindService();
        void UnBindService();
        void StartLocationRequest();
        void StopLocationRequest();
        void DisableListener();
    }
}
