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
		public static readonly DependencyProperty AccountancyApplicationProperty
			= DependencyProperty.Register("AccountancyApplication", typeof(AccountancyApplication), typeof(MainWindow));

		public AccountancyApplication AccountancyApplication
		{
			get { return (AccountancyApplication)GetValue(AccountancyApplicationProperty); }
			private set { SetValue(AccountancyApplicationProperty, value); }
		}

        public static readonly DependencyProperty DatabaseFileNameProperty =
			DependencyProperty.Register("DatabaseFileName", typeof(string), typeof(MainWindow));

		public string DatabaseFileName
		{
			get { return (string)GetValue(DatabaseFileNameProperty); }
			private set { SetValue(DatabaseFileNameProperty, value); }
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

			AccountancyApplication = new AccountancyApplication(conn);
		}

		public CategoryViewModel SelectedCategory
		{
			get { return treeCategories.SelectedValue as CategoryViewModel; }
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			InputDialog dlg = new InputDialog();
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddCategory(dlg.Value);
		}

		private void btnAddChild_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddChildCategory(SelectedCategory, dlg.Value);
		}

		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			dlg.Value = SelectedCategory.UnderlyingData.name;
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.RenameCategory(SelectedCategory, dlg.Value);
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			AccountancyApplication.RemoveCategory(SelectedCategory);
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void btnAddUser_Click(object sender, RoutedEventArgs e)
		{
			UserInfoDialog dlg = new UserInfoDialog(AccountancyApplication.NewPerson());
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddPerson(dlg.PersonInfo);
		}

		private void btnEditUser_Click(object sender, RoutedEventArgs e)
		{
			if (null == AccountancyApplication.SelectedUser)
				return;
			UserInfoDialog dlg = new UserInfoDialog(AccountancyApplication.SelectedUser);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.UpdatePerson(dlg.PersonInfo);
		}

		private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
		{
			if (null == AccountancyApplication.SelectedUser)
				return;
			AccountancyApplication.RemovePerson(AccountancyApplication.SelectedUser);
		}

		private void btnAccountAdd_Click(object sender, RoutedEventArgs e)
		{
			AccountDialog dlg = new AccountDialog(AccountancyApplication.SelectedUser, AccountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddAccount(dlg.AccountInfo);
		}

		private void btnAccountChange_Click(object sender, RoutedEventArgs e)
		{
			if (null == AccountancyApplication.SelectedAccount)
				return;
			AccountDialog dlg = new AccountDialog(AccountancyApplication.SelectedAccount, AccountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.UpdateAccount(AccountancyApplication.SelectedAccount);
		}

		private void btnAccountRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == AccountancyApplication.SelectedAccount)
				return;
			AccountancyApplication.DeleteAccount(AccountancyApplication.SelectedAccount);
		}

		private void btnExpenseAdd_Click(object sender, RoutedEventArgs e)
		{
			CategoryViewModel selectedCategory = cboExpenseCategory.SelectedItem as CategoryViewModel;
			if (null == selectedCategory)
				return;
			Expense ex = new Expense(0, AccountancyApplication.SelectedAccount.AccountId,
									long.Parse(txtExpenseAmount.Text), selectedCategory.CategoryId,
									AccountancyApplication.SelectedDate, txtExpenseDescription.Text);
			AccountancyApplication.AddExpense(new ExpenseViewModel(ex, AccountancyApplication));
		}

		public ExpenseViewModel SelectedExpense
		{
			get { return dgExpenses.SelectedItem as ExpenseViewModel; }
		}

		private void btnExpenseChange_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedExpense)
				return;
			CategoryViewModel selectedCategory = cboExpenseCategory.SelectedItem as CategoryViewModel;
			if (null == selectedCategory)
				return;
			SelectedExpense.Account = AccountancyApplication.SelectedAccount;
			SelectedExpense.Description = txtExpenseDescription.Text;
			SelectedExpense.Amount = long.Parse(txtExpenseAmount.Text);
			SelectedExpense.Date = AccountancyApplication.SelectedDate;
			SelectedExpense.Category = selectedCategory;

			AccountancyApplication.UpdateExpense(SelectedExpense);
		}

		private void btnExpenseRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedExpense)
				return;
			AccountancyApplication.DeleteExpense(SelectedExpense);
		}
	}

	public class MoneyAmountTextToColorConverter : IValueConverter
	{
		private static MoneyAmountTextToColorConverter m_instance;

		public static MoneyAmountTextToColorConverter Instance
		{
			get
			{
				if (null == m_instance)
					m_instance = new MoneyAmountTextToColorConverter();
				return m_instance;
			}
		}

		private Brush GetBrush(bool isNegative)
		{
			return isNegative ? Brushes.Red : Brushes.Green;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = value as string;
			if (!str.IsNull())
				return GetBrush(!str.IsEmpty() ? ('-' == str[0]) : false);
			int v = (int)value;
			return GetBrush(v < 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("ConvertBack");
		}
	}
}
