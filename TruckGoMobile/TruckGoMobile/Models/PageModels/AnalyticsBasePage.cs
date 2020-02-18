using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TruckGoMobile.Interfaces;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class AnalyticsBasePage : LoadingContentPage
    {
        public readonly string pageName;

        List<Button> analyticButtons;

        public AnalyticsBasePage(string pageName)
        {
            this.pageName = pageName;
        }

        public void SetAnalyticButtons(params Button[] buttons)
        {
            if (analyticButtons != null)
                UnRegisterButtonAnalyticsEvents();

            analyticButtons = new List<Button>(buttons);
            RegisterButtonAnalyticsEvents();
        }

        void UnRegisterButtonAnalyticsEvents()
        {
            foreach (var button in analyticButtons)
                button.Clicked -= AnalyticsButton_Clicked;
        }

        void RegisterButtonAnalyticsEvents()
        {
            foreach (var button in analyticButtons)
                button.Clicked += AnalyticsButton_Clicked;
        }

        private void AnalyticsButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IFirebaseAnalytics>().SendEvent(pageName + (sender as Button).Text + "Clicked");/*, new Dictionary<string, string>*/
            //{
            //    { "NameSurname",UserManager.Instance.CurrentLoggedInUser.UserNameSurname }
            //});
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DependencyService.Get<IFirebaseAnalytics>().SendEvent(pageName + "Appearing", new Dictionary<string, string>
            {
                {"NameSurname",UserManager.Instance.CurrentLoggedInUser.UserNameSurname },

            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnRegisterButtonAnalyticsEvents();
            DependencyService.Get<IFirebaseAnalytics>().SendEvent(pageName + "Disappearing", new Dictionary<string, string>
            {
                { "NameSurname",UserManager.Instance.CurrentLoggedInUser.UserNameSurname }
            });
        }
    }
}
