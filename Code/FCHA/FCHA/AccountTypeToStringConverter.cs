using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace FCHA
{
    public class AccountTypeToStringConverter : IValueConverter
    {
        private static AccountTypeToStringConverter m_instance;

        public static AccountTypeToStringConverter Instance
        {
            get
            {
                if (null == m_instance)
                    m_instance = new AccountTypeToStringConverter();
                return m_instance;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AccountType type = (AccountType)value;
            switch (type)
            {
                case AccountType.Cash:
                    return App.ThisApp.AccountTypeCash;
                case AccountType.CreditCard:
                    return App.ThisApp.AccountTypeCreditCard;
                case AccountType.DebetCard:
                    return App.ThisApp.AccountTypeDebetCard;
                case AccountType.Deposit:
                    return App.ThisApp.AccountTypeDeposit;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack");
        }
    }
}
