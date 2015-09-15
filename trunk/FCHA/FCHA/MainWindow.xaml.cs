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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Diagnostics;
using System.ComponentModel;

using IOPath = System.IO.Path;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace FCHA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
		: Window
	{
		private AccountancyApplication m_accountancyApplication;

		public static readonly DependencyProperty VirtualRootProperty =
			DependencyProperty.Register("VirtualRoot", typeof(CategoryViewModel), typeof(MainWindow));
			
		public CategoryViewModel VirtualRoot
		{
			get { return (CategoryViewModel)GetValue(VirtualRootProperty); }
			private set { SetValue(VirtualRootProperty, value); }
		}

		public static readonly DependencyProperty DatabaseFileNameProperty =
			DependencyProperty.Register("DatabaseFileName", typeof(string), typeof(MainWindow));

		public string DatabaseFileName
		{
			get { return (string)GetValue(DatabaseFileNameProperty); }
			private set { SetValue(DatabaseFileNameProperty, value); }
		}

		public static readonly DependencyProperty UsersProperty =
			DependencyProperty.Register("Users", typeof(List<PersonViewModel>), typeof(MainWindow));

		public List<PersonViewModel> Users
		{
			get { return (List<PersonViewModel>)GetValue(UsersProperty); }
			private set { SetValue(UsersProperty, value); }
		}
		
		public MainWindow()
		{
			InitializeComponent();

			Application.Current.DispatcherUnhandledException += (e, a) =>
				{
					if (a.Exception is SQLiteException)
					{
						SQLiteException ex = (SQLiteException)a.Exception;
						MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
						a.Handled = true;
					}
				};

			DatabaseFileName = IOPath.GetFullPath("..\\..\\..\\..\\..\\Data\\FCHA_Master");

			SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DatabaseFileName));
			conn.Open();

			m_accountancyApplication = new AccountancyApplication(conn);

			VirtualRoot = m_accountancyApplication.CategoriesRoot;
			UpdateUsers();
		}

		private void UpdateUsers()
		{
			Users = null;
			Users = m_accountancyApplication.Users;
		}

		public CategoryViewModel SelectedCategory
		{
			get { return treeCategories.SelectedValue as CategoryViewModel; }
		}

		public PersonViewModel SelectedUser
		{
			get { return cboUsers.SelectedValue as PersonViewModel; }
		}

		public AccountViewModel SelectedAccount
		{
			get { return listAccounts.SelectedValue as AccountViewModel; }
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			InputDialog dlg = new InputDialog();
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.AddCategory(dlg.Value);
		}

		private void btnAddChild_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.AddChildCategory(SelectedCategory, dlg.Value);
		}

		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			dlg.Value = SelectedCategory.UnderlyingData.name;
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.RenameCategory(SelectedCategory, dlg.Value);
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			m_accountancyApplication.RemoveCategory(SelectedCategory);
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void btnAddUser_Click(object sender, RoutedEventArgs e)
		{
			UserInfoDialog dlg = new UserInfoDialog(m_accountancyApplication.NewPerson());
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.AddPerson(dlg.PersonInfo);
			UpdateUsers();
		}

		private void btnEditUser_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedUser)
				return;
			UserInfoDialog dlg = new UserInfoDialog(SelectedUser);
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.UpdatePerson(dlg.PersonInfo);
			UpdateUsers();
		}

		private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedUser)
				return;
			m_accountancyApplication.RemovePerson(SelectedUser);
			UpdateUsers();
		}

		private void UpdateSelectedUserHack()
		{
			object si = cboUsers.SelectedItem;
			cboUsers.SelectedItem = null;
			cboUsers.SelectedItem = si;
		}

		private void btnAccountAdd_Click(object sender, RoutedEventArgs e)
		{
			AccountDialog dlg = new AccountDialog(m_accountancyApplication.CreateAccount(SelectedUser), m_accountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.AddAccount(dlg.AccountInfo);
			UpdateSelectedUserHack();
		}

		private void btnAccountChange_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedAccount)
				return;
			AccountDialog dlg = new AccountDialog(SelectedAccount, m_accountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			m_accountancyApplication.UpdateAccount(SelectedAccount);
			UpdateSelectedUserHack();
		}

		private void btnAccountRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedAccount)
				return;
			m_accountancyApplication.DeleteAccount(SelectedAccount);
			UpdateSelectedUserHack();
		}
	}

	public class SelectedUserToAccountsConverter : IValueConverter
	{
		private static SelectedUserToAccountsConverter m_instance;

		public static SelectedUserToAccountsConverter Instance
		{
			get
			{
				if (null == m_instance)
					m_instance = new SelectedUserToAccountsConverter();
				return m_instance;
			}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			PersonViewModel vm = value as PersonViewModel;
			if (null == vm)
				return null;
			return vm.GetUserAccounts();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("ConvertBack");
		}
	}

	public class DebugConverter : IValueConverter
	{
		private static DebugConverter m_instance;

		public static DebugConverter Instance
		{
			get
			{
				if (null == m_instance)
					m_instance = new DebugConverter();
				return m_instance;
			}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
