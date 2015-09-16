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
			if (null == SelectedUser)
				return;
			UserInfoDialog dlg = new UserInfoDialog(SelectedUser);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.UpdatePerson(dlg.PersonInfo);
		}

		private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedUser)
				return;
			AccountancyApplication.RemovePerson(SelectedUser);
		}

		private void UpdateSelectedUserHack()
		{
			object si = cboUsers.SelectedItem;
			cboUsers.SelectedItem = null;
			cboUsers.SelectedItem = si;
		}

		private void btnAccountAdd_Click(object sender, RoutedEventArgs e)
		{
			AccountDialog dlg = new AccountDialog(SelectedUser, AccountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.AddAccount(dlg.AccountInfo);
			UpdateSelectedUserHack();
		}

		private void btnAccountChange_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedAccount)
				return;
			AccountDialog dlg = new AccountDialog(SelectedAccount, AccountancyApplication);
			if (true != dlg.ShowDialog())
				return;
			AccountancyApplication.UpdateAccount(SelectedAccount);
			UpdateSelectedUserHack();
		}

		private void btnAccountRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedAccount)
				return;
			AccountancyApplication.DeleteAccount(SelectedAccount);
			UpdateSelectedUserHack();
		}
	}
}
