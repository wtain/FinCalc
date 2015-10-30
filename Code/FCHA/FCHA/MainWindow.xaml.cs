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
using System.Threading;
using FCHA.Dialogs;

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
            
			AccountancyApplication = new AccountancyApplication(new AccountancyDatabase(conn), new CbrClient());
		}

		public CategoryViewModel SelectedCategory
		{
			get { return treeCategories.SelectedValue as CategoryViewModel; }
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			CategoryDialog dlg = new CategoryDialog();
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddCategory(dlg.CategoryName, dlg.IsIncome);
		}

		private void btnAddChild_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
            CategoryDialog dlg = new CategoryDialog();
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddChildCategory(SelectedCategory, dlg.CategoryName, dlg.IsIncome);
		}

		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
            CategoryDialog dlg = new CategoryDialog();
			dlg.CategoryName = SelectedCategory.UnderlyingData.name;
            dlg.IsIncome = SelectedCategory.UnderlyingData.isIncome;
            if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.ChangeCategory(SelectedCategory, dlg.CategoryName, dlg.IsIncome);
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

        private void btnAccountTransfer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLanguageRussian_Click(object sender, RoutedEventArgs e)
        {
            App.ThisApp.SetLanguage("ru-RU");
        }

        private void btnLanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            App.ThisApp.SetLanguage("en-US");
        }

        private CategoryViewModel CategoryFromTextBlockSender(object sender)
        {
            FrameworkElement fe = sender as FrameworkElement;
            if (null == fe)
                return null;
            return fe.DataContext as CategoryViewModel;
        }

        private void tbCategory_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CategoryViewModel category = CategoryFromTextBlockSender(sender);
                if (null != category)
                {
                    DataObject dataObj = new DataObject();
                    dataObj.SetData("Category", category);
                    DragDrop.DoDragDrop(treeCategories, dataObj, DragDropEffects.Move);
                }
            }
        }

        private void tbCategory_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Category"))
            {
                CategoryViewModel category = (CategoryViewModel)e.Data.GetData("Category");

                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }

        private void CheckCanDrop(CategoryViewModel currentCategory, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (null == currentCategory)
                return;
            if (!e.Data.GetDataPresent("Category"))
                return;
            e.Handled = true;
            CategoryViewModel category = (CategoryViewModel)e.Data.GetData("Category");
            if (null == category)
                return;
            if (category.IsCovers(currentCategory))
                return;
            e.Effects = DragDropEffects.Move;
        }

        private void tbCategory_DragOver(object sender, DragEventArgs e)
        {
            CheckCanDrop(CategoryFromTextBlockSender(sender), e);
        }

        private void treeCategories_Drop(object sender, DragEventArgs e)
        {

        }

        private void treeCategories_DragOver(object sender, DragEventArgs e)
        {
            CheckCanDrop(AccountancyApplication.VirtualRoot, e);
        }
    }
}
