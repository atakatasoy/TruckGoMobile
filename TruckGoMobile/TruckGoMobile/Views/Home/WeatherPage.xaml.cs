﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views.Home
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WeatherPage : AnalyticsBasePage
	{
        public WeatherPage() : base(nameof(WeatherPage))
        {
            InitializeComponent();
        }
	}
}