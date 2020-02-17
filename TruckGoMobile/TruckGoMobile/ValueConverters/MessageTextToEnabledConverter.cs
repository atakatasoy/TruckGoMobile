using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TruckGoMobile.ValueConverters;

namespace TruckGoMobile
{
    public class MessageTextToEnabledConverter : BaseValueConverter<MessageTextToEnabledConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var buffer=value as string;

            if (string.IsNullOrWhiteSpace(buffer))
                return false;

            return true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
