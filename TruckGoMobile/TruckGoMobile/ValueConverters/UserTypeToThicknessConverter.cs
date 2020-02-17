using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TruckGoMobile.ValueConverters;
using Xamarin.Forms;

namespace TruckGoMobile
{
    public class UserTypeToThicknessConverter : BaseValueConverter<UserTypeToThicknessConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userName = value as string;

            if (userName == UserManager.Instance.CurrentLoggedInUser.UserNameSurname)
                return new Thickness(40, 1, 0, 1);

            return new Thickness(0, 1, 40, 1);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
