using System;
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
		private UsersManager m_usersManager;
		private AccountsManager m_accountsManager;

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

		public AccountDialog(UsersManager usersManager, AccountsManager accountsManager, AccountViewModel account)
		{
			m_usersManager = usersManager;
			m_accountsManager = accountsManager;
			Users = new List<PersonViewModel>(m_usersManager.EnumAllUsers().Select(p => new PersonViewModel(p, m_accountsManager, m_usersManager)));
			AccountInfo = account;
			if (null != AccountInfo.Owner)
				AccountInfo.Owner = Users.Find(p => { return p.UnderlyingData.personId == account.Owner.UnderlyingData.personId; });
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
