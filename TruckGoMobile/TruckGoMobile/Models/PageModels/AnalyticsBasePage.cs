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
        List<IAnalyticInteractional> analyticInteractionalViews;
        IFirebaseAnalytics firebase = DependencyService.Get<IFirebaseAnalytics>();

        public AnalyticsBasePage(string pageName)
        {
            this.pageName = pageName;
        }

        public void SetInteractionalComponents(params IAnalyticInteractional[] buttons)
        {
            if (analyticInteractionalViews != null)
                UnRegisterInteractionalComponentEvents();

            analyticInteractionalViews = new List<IAnalyticInteractional>(buttons);
            RegisterInteractionalComponentEvents();
        }

        void UnRegisterInteractionalComponentEvents()
        {
            foreach (var button in analyticInteractionalViews)
                button.UnregisterMethod(AnalyticsButton_Clicked);
            
        }

        public void AddInteractionalComponent(IAnalyticInteractional button)
        {
            if (analyticInteractionalViews == null)
                analyticInteractionalViews = new List<IAnalyticInteractional>();

            analyticInteractionalViews.Add(button);
            RegisterInteractionalComponentEvents(button);
        }

        void RegisterInteractionalComponentEvents(IAnalyticInteractional button)
        {
            button.RegisterMethod(AnalyticsButton_Clicked);
        }

        void RegisterInteractionalComponentEvents()
        {
            foreach (var component in analyticInteractionalViews)
                component.RegisterMethod(AnalyticsButton_Clicked);
        }

        private void AnalyticsButton_Clicked(object sender, EventArgs e)
        {
            var senderr = sender as Button;
            Task.Run(() => firebase.SendEvent(pageName + ((sender as IAnalyticInteractional).Identifier ?? "Unknown") + "Clicked", new Dictionary<string, string>
            {
                { "NameSurname",UserManager.Instance.CurrentLoggedInUser?.UserNameSurname },
            }));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(() => firebase.SendEvent(pageName + "Appearing", new Dictionary<string, string>
            {
                {"NameSurname",UserManager.Instance.CurrentLoggedInUser?.UserNameSurname },

            }));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnRegisterInteractionalComponentEvents();
            Task.Run(() => firebase.SendEvent(pageName + "Disappearing", new Dictionary<string, string>
            {
                { "NameSurname",UserManager.Instance.CurrentLoggedInUser?.UserNameSurname }
            }));
        }
    }
}
