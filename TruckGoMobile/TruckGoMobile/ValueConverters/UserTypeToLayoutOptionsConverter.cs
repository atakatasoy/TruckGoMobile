using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TruckGoMobile.ValueConverters;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class UserTypeToLayoutOptionsConverter : BaseValueConverter<UserTypeToLayoutOptionsConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nameSurname = value as string;

            if (nameSurname == UserManager.Instance.CurrentLoggedInUser.UserNameSurname)
                return LayoutOptions.EndAndExpand;

            return LayoutOptions.StartAndExpand;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
