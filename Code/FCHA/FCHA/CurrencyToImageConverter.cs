using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FCHA
{
    public class CurrencyToImageConverter : IValueConverter
    {
        private static CurrencyToImageConverter m_instance;

        public static CurrencyToImageConverter Instance
        {
            get
            {
                if (null == m_instance)
                    m_instance = new CurrencyToImageConverter();
                return m_instance;
            }
        }

        private ImageSource m_EURImage;
        private ImageSource m_RUBImage;
        private ImageSource m_USDImage;
        private ImageSource m_GBPImage;

        public ImageSource EURImage
        {
            get
            {
                if (null == m_EURImage)
                    m_EURImage = (ImageSource)Application.Current.FindResource("EURIcon");
                return m_EURImage;
            }
        }

        public ImageSource RUBImage
        {
            get
            {
                if (null == m_RUBImage)
                    m_RUBImage = (ImageSource)Application.Current.FindResource("RUBIcon");
                return m_RUBImage;
            }
        }

        public ImageSource USDImage
        {
            get
            {
                if (null == m_USDImage)
                    m_USDImage = (ImageSource)Application.Current.FindResource("USDIcon");
                return m_USDImage;
            }
        }

        public ImageSource GBPImage
        {
            get
            {
                if (null == m_GBPImage)
                    m_GBPImage = (ImageSource)Application.Current.FindResource("GBPIcon");
                return m_GBPImage;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currency = (string)value;
            if (currency == "EUR")
                return EURImage;
            else if (currency == "RUB")
                return RUBImage;
            else if (currency == "USD")
                return USDImage;
            else if (currency == "GBP")
                return GBPImage;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
