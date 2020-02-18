using System;
using System.Collections.Generic;
using System.Text;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class AnalyticButton : Button, IAnalyticInteractional
    {
        public string Identifier { get; set; }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            Identifier = Text;
        }

        public void RegisterMethod(Action<object, EventArgs> method)
        {
            var handlerToRegister = new EventHandler(method);
            Clicked += handlerToRegister;
        }

        public void UnregisterMethod(Action<object,EventArgs> method)
        {
            var handlerToUnregister = new EventHandler(method);
            Clicked -= handlerToUnregister;
        }
    }
}
