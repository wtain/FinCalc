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

namespace FCHA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
		: Window
	{
		private CategoriesManager m_mgr;

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

			m_mgr = new CategoriesManager(conn);

			VirtualRoot = new CategoryViewModel(m_mgr, null, new Category("Virtual", 0));
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
			VirtualRoot.RefreshChildren();
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
			Debug.Assert(null != SelectedCategory && null != SelectedCategory.Parent);
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
