using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using TruckGoMobile.ValueConverters;

namespace TruckGoMobile
{
    public class PathStringToColorConverter : BaseValueConverter<PathStringToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringVal = value as string;

            if (value != null)
                return Color.Green;

            return Color.Red;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
