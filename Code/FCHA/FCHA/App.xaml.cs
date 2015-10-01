using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace FCHA
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static Style m_MoneyAmountTextStyle;
		private static Style m_CellStyle;

        public static Style MoneyAmountTextStyle
		{
			get
			{
				if (null == m_MoneyAmountTextStyle)
					m_MoneyAmountTextStyle = (Style)Current.FindResource("MoneyAmountTextStyle");
				return m_MoneyAmountTextStyle;
			}
		}

        public static Style CellStyle
        {
            get
            {
                if (null == m_CellStyle)
                    m_CellStyle = (Style)Current.FindResource("CellStyle");
                return m_CellStyle;
            }
        }
        
    }
}
