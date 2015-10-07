using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FCHA.DataTypes
{
    public static class AccountTypeHelper
    {
        public static string AccountTypeToString(AccountType type)
        {
            switch (type)
            {
                case AccountType.Cash:
                    return "CASH";
                case AccountType.CreditCard:
                    return "CREDITCARD";
                case AccountType.DebetCard:
                    return "DEBETCARD";
                case AccountType.Deposit:
                    return "DEPOSIT";
            }
            Debug.Assert(false);
            return string.Empty;
        }

        public static AccountType AccountTypeFromString(string strType)
        {
            switch (strType)
            {
                case "CASH":
                    return AccountType.Cash;
                case "CREDITCARD":
                    return AccountType.CreditCard;
                case "DEBETCARD":
                    return AccountType.DebetCard;
                case "DEPOSIT":
                    return AccountType.Deposit;
            }
            Debug.Assert(false);
            return AccountType.Cash;
        }
    }
}
