using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using TruckGoMobile.ValueConverters;

namespace TruckGoMobile
{
    public class BoolToRecordingBackgroundColorConverter : BaseValueConverter<BoolToRecordingBackgroundColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolVal = (bool)value;

            if (boolVal)
            {
                return Color.Red;
            }
            return Color.Green;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
