﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FCHA
{
	/// <summary>
	/// Interaction logic for AccountDialog.xaml
	/// </summary>
	public partial class AccountDialog : Window
	{
		public static readonly DependencyProperty AccountInfoProperty =
			DependencyProperty.Register("AccountInfo", typeof(AccountViewModel), typeof(AccountDialog));

		public static readonly DependencyProperty UsersProperty =
			DependencyProperty.Register("Users", typeof(List<PersonViewModel>), typeof(AccountDialog));

		public AccountViewModel AccountInfo
		{
			get { return (AccountViewModel)GetValue(AccountInfoProperty); }
			set { SetValue(AccountInfoProperty, value); }
		}

		public List<PersonViewModel> Users
		{
			get { return (List<PersonViewModel>)GetValue(UsersProperty); }
			set { SetValue(UsersProperty, value); }
		}

		public AccountDialog(AccountViewModel account, AccountancyApplication accountancyApplication)
		{
			Users = accountancyApplication.Users;
			AccountInfo = account;
			InitializeComponent();
			txtName.SelectAll();
		}

		public static Array AccountTypes
		{
			get { return Enum.GetValues(typeof(AccountType)); }
		}

		private static List<string> s_currencies;

		public static List<string> Currencies
		{
			get 
			{
				if (null == s_currencies)
				{
					s_currencies = new List<string>();
					s_currencies.AddRange(new string[] {"RUB", "EUR", "USD", "GBP"});
				}
				return s_currencies;
			}
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			AccountInfo.UpdateUnderlyingData();
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
