using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TruckGoMobile.CustomRenderers;
using TruckGoMobile.Droid.CustomComponents;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoUnfocusEntry), typeof(NoUnfocusEntryRenderer))]
namespace TruckGoMobile.Droid.CustomComponents
{
    public class NoUnfocusEntryRenderer : EntryRenderer
    {
        public NoUnfocusEntryRenderer(Context context) : base(context)
        {
        }
    }
}