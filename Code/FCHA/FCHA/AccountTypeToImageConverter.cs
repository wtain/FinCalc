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
    public class AccountTypeToImageConverter : IValueConverter
    {
        private static AccountTypeToImageConverter m_instance;

        public static AccountTypeToImageConverter Instance
        {
            get
            {
                if (null == m_instance)
                    m_instance = new AccountTypeToImageConverter();
                return m_instance;
            }
        }

        private ImageSource m_cardImage;
        private ImageSource m_cashImage;
        private ImageSource m_depositImage;

        public ImageSource CardImage
        {
            get
            {
                if (null == m_cardImage)
                    m_cardImage = (ImageSource)Application.Current.FindResource("CreditCardIcon");
                return m_cardImage;
            }
        }

        public ImageSource CashImage
        {
            get
            {
                if (null == m_cashImage)
                    m_cashImage = (ImageSource)Application.Current.FindResource("CoinsIcon");
                return m_cashImage;
            }
        }

        public ImageSource DepositImage
        {
            get
            {
                if (null == m_depositImage)
                    m_depositImage = (ImageSource)Application.Current.FindResource("CashRegisterIcon");
                return m_depositImage;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccountType type = (AccountType)value;
            switch (type)
            {
                case AccountType.Cash:
                    return CashImage;
                case AccountType.CreditCard:
                case AccountType.DebetCard:
                    return CardImage;
                case AccountType.Deposit:
                    return DepositImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
