﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public partial class LoadingContentPage : ContentPage
    {
        public delegate Task WebServiceDataRetrieveHandlers();
        public event WebServiceDataRetrieveHandlers ReadyToLoadData;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext != null && BindingContext is IServiceInfo)
                if (((IServiceInfo)BindingContext).GetServiceMethods() is WebServiceDataRetrieveHandlers d)
                    ReadyToLoadData += d;
            
            if (ReadyToLoadData != null)
            {
                await DialogManager.Instance.ShowIndicatorAsync();
                await ReadyToLoadData?.Invoke();
                DialogManager.Instance.HideIndicator();
            }           
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (ReadyToLoadData != null)
                foreach (var eachHandler in ReadyToLoadData.GetInvocationList())
                    ReadyToLoadData -= (WebServiceDataRetrieveHandlers)eachHandler;
        }
    }
}
