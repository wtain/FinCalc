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

namespace FCHA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
		: Window
		, INotifyPropertyChanged
	{
		public IEnumerable<CategoryViewModel> TopLevelCategories { get; private set; }

		private PropertyChangedEventHandler propertyChangedHandlers;

		private void FirePropertyChanged(string propertyName)
		{
			if (null == propertyChangedHandlers)
				return;
			propertyChangedHandlers(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				if (null == propertyChangedHandlers)
					propertyChangedHandlers = (PropertyChangedEventHandler)MulticastDelegate.Combine(value);
				else
					propertyChangedHandlers = (PropertyChangedEventHandler)MulticastDelegate.Combine(propertyChangedHandlers, value);
			}
			remove
			{
				propertyChangedHandlers = (PropertyChangedEventHandler)MulticastDelegate.Remove(propertyChangedHandlers, value);
			}
		}

		private CategoriesManager m_mgr;
		
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


			SQLiteConnection conn = new SQLiteConnection("Data Source=..\\..\\..\\..\\..\\Data\\FCHA_Master;Version=3;");
			conn.Open();

			m_mgr = new CategoriesManager(conn);

			RefreshCategories();
		}

		private void RefreshCategories()
		{
			TopLevelCategories = new List<CategoryViewModel>(m_mgr.EnumCategoriesByParent(0).Select(c => new CategoryViewModel(m_mgr, null, c)));
			FirePropertyChanged("TopLevelCategories");
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
			m_mgr.AddCategory(dlg.Value);
			RefreshCategories();
		}

		private void btnAddChild_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			if (true != dlg.ShowDialog())
				return;
			m_mgr.AddCategory(dlg.Value, SelectedCategory.UnderlyingData.categoryId);
			SelectedCategory.RefreshChildren();
		}

		private void btnRename_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			InputDialog dlg = new InputDialog();
			dlg.Value = SelectedCategory.UnderlyingData.name;
			if (true != dlg.ShowDialog())
				return;
			Category cat = SelectedCategory.UnderlyingData;
			cat.name = dlg.Value;
			m_mgr.UpdateCategory(cat);
			RefreshCurrentLevel();
		}

		private void RefreshCurrentLevel()
		{
			if (null == SelectedCategory.Parent)
				RefreshCategories();
			else
				SelectedCategory.RefreshParentChildren();
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			if (null == SelectedCategory)
				return;
			m_mgr.DeleteCategory(SelectedCategory.UnderlyingData);
			RefreshCurrentLevel();
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
