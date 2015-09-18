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

		public static Style MoneyAmountTextStyle
		{
			get
			{
				if (null == m_MoneyAmountTextStyle)
					m_MoneyAmountTextStyle = (Style)Current.FindResource("MoneyAmountTextStyle");
				return m_MoneyAmountTextStyle;
			}
		}
	}
}
