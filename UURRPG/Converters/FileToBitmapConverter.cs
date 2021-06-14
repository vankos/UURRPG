using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace UURRPG.Converters
{
    public class FileToBitmapConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> _locationsImages = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string filename)) return null;

            if (!_locationsImages.ContainsKey(filename))
            {
                _locationsImages.Add(filename,
                    new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}{filename}", UriKind.Absolute)));
            }

            return _locationsImages[filename];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
