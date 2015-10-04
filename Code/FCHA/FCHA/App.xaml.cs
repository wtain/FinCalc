using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
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

        public App()
            : base()
        {
            SetLanguageDictionary(Thread.CurrentThread.CurrentCulture);
        }

        public void SetLanguageDictionary(CultureInfo ci)
        {
            string dictUri = string.Format("..\\Resources\\GUIStrings_{0}.xaml", ci.ToString());
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                dict.Source = new Uri(dictUri, UriKind.Relative);
            }
            catch (IOException)
            {
                dict.Source = new Uri("..\\Resources\\GUIStrings.xaml", UriKind.Relative);
            }
            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(dict);
        }

        public ResourceDictionary CurrentDictionary
        {
            get { return this.Resources.MergedDictionaries[0]; }
        }

        public string ExpenseByCategory
        {
            get { return (string) CurrentDictionary["ExpenseByCategory"]; }
        }

        public string ExpenseByCategoryAndMonth
        {
            get { return (string)CurrentDictionary["ExpenseByCategoryAndMonth"]; }
        }

        public string ExpenseByTopLevelCategoryAndMonth
        {
            get { return (string)CurrentDictionary["ExpenseByTopLevelCategoryAndMonth"]; }
        }

        public static App ThisApp
        {
            get { return (App)App.Current; }
        }

        public void SetLanguage(string language)
        {
            CultureInfo ci = CultureInfo.GetCultureInfo(language);
            SetLanguageDictionary(ci);
            Thread.CurrentThread.CurrentCulture = ci;
        }
    }
}
