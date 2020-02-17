using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TruckGoMobile.ValueConverters
{
    public abstract class BaseValueConverter<T> : IMarkupExtension, IValueConverter
         where T : class, new()
    {
        #region Private Members

        private static T mConverter = null;

        #endregion

        #region MarkupExtension Methods

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T());
        }

        #endregion

        #region Converter Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion

    }
}
