using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TruckGoMobile.Interfaces
{
    public interface IAnalyticInteractional
    {
        string Identifier { get; set; }
        void RegisterMethod(Action<object, EventArgs> method);
        void UnregisterMethod(Action<object, EventArgs> method);
    }
}
