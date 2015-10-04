using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace FCHA
{
    public class MoneyAmountTextToColorConverter : IValueConverter
    {
        private static MoneyAmountTextToColorConverter m_instance;

        public static MoneyAmountTextToColorConverter Instance
        {
            get
            {
                if (null == m_instance)
                    m_instance = new MoneyAmountTextToColorConverter();
                return m_instance;
            }
        }

        private Brush GetBrush(bool isNegative)
        {
            return isNegative ? Brushes.Red : Brushes.Green;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            if (!str.IsNull())
                return GetBrush(!str.IsEmpty() ? ('-' == str[0]) : false);
            int v = (int)value;
            return GetBrush(v < 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
