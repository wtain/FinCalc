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
	/// Interaction logic for UserInfoDialog.xaml
	/// </summary>
	public partial class UserInfoDialog : Window
	{
		public static readonly DependencyProperty PersonInfoProperty =
			DependencyProperty.Register("PersonInfo", typeof(PersonViewModel), typeof(UserInfoDialog));

		public PersonViewModel PersonInfo
		{
			get { return (PersonViewModel)GetValue(PersonInfoProperty); }
			set { SetValue(PersonInfoProperty, value); }
		}

		public UserInfoDialog(AccountsManager accountsManager, UsersManager usersManager)
			: this(new Person("Name", "Full Name", 0), accountsManager, usersManager)
		{
		}

		public UserInfoDialog(Person person, AccountsManager accountsManager, UsersManager usersManager)
			: this(new PersonViewModel(person, accountsManager, usersManager))
		{
		}

		public UserInfoDialog(PersonViewModel personVM)
		{
			PersonInfo = personVM;
			InitializeComponent();
			txtUserName.Focus();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			PersonInfo.UpdateUnderlyingData();
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void txtUserName_GotFocus(object sender, RoutedEventArgs e)
		{
			txtUserName.SelectAll();
		}

		private void txtFullName_GotFocus(object sender, RoutedEventArgs e)
		{
			txtFullName.SelectAll();
		}

		private void dlgUserInput_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			txtUserName.SelectAll();
		}
	}
}
