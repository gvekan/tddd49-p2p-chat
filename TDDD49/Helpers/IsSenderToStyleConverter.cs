using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TDDD49.Helpers
{
    class IsSenderToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Style style = new Style(typeof(Grid));
            bool IsSender = (bool)value;
            Brush brush = IsSender ? Brushes.LightGray : Brushes.LightBlue;
            style.Setters.Add(new Setter(Grid.BackgroundProperty, brush));
            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
