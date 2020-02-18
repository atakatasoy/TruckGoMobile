using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.Views.PartialView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleButton : ContentView, IAnalyticInteractional
    {
        string mName;
        public string Name
        {
            get { return mName; }
            set
            {
                Identifier = value;
                mName = value;
            }
        }
        public string Identifier { get; set; }
        public ICommand ClickedCommand { get; set; }
        public event EventHandler Clicked;

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(CircleButton),
            default(ImageSource),
            BindingMode.TwoWay);
        public ImageSource ImageSource { get { return (ImageSource)GetValue(ImageSourceProperty); } set { SetValue(ImageSourceProperty, value); } }

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
            nameof(LabelText),
            typeof(string),
            typeof(CircleButton),
            default(string),
            BindingMode.TwoWay);
        public string LabelText { get { return (string)GetValue(LabelTextProperty); } set { SetValue(LabelTextProperty, value); } }

        public CircleButton()
        {
            InitializeComponent();
            ClickedCommand = new Command(() => Clicked?.Invoke(this, null));
            BindingContext = this;
        }

        public void RegisterMethod(Action<object, EventArgs> method)
        {
            var methodToRegister = new EventHandler(method);
            Clicked += methodToRegister;
        }

        public void UnregisterMethod(Action<object, EventArgs> method)
        {
            var methodToUnregister = new EventHandler(method);
            Clicked -= methodToUnregister;
        }
    }
}