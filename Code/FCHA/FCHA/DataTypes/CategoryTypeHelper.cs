using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FCHA.DataTypes
{
    public static class CategoryTypeHelper
    {
        public static string CategoryTypeToString(CategoryType type)
        {
            switch (type)
            {
                case CategoryType.Expense:
                    return "Exp";
                case CategoryType.Income:
                    return "Inc";
                case CategoryType.TransferIn:
                    return "Tri";
                case CategoryType.TransferOut:
                    return "Tro";
            }
            Debug.Assert(false);
            return string.Empty;
        }

        public static CategoryType CategoryTypeFromString(string strType)
        {
            switch (strType)
            {
                case "Exp":
                    return CategoryType.Expense;
                case "Inc":
                    return CategoryType.Income;
                case "Tri":
                    return CategoryType.TransferIn;
                case "Tro":
                    return CategoryType.TransferOut;
            }
            Debug.Assert(false);
            return CategoryType.Expense;
        }

        public static bool IsPositive(CategoryType type)
        {
            switch (type)
            {
                case CategoryType.Expense:
                case CategoryType.TransferOut:
                    return false;
                case CategoryType.Income:
                case CategoryType.TransferIn:
                    return true;
            }
            Debug.Assert(false);
            return false;
        }

        public static bool IsNegative(CategoryType type)
        {
            switch (type)
            {
                case CategoryType.Expense:
                case CategoryType.TransferOut:
                    return true;
                case CategoryType.Income:
                case CategoryType.TransferIn:
                    return false;
            }
            Debug.Assert(false);
            return false;
        }

        public static int Sign(CategoryType type)
        {
            switch (type)
            {
                case CategoryType.Expense:
                case CategoryType.TransferOut:
                    return -1;
                case CategoryType.Income:
                case CategoryType.TransferIn:
                    return 1;
            }
            Debug.Assert(false);
            return 0;
        }
    }
}
