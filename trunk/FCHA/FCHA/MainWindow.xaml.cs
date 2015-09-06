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

namespace FCHA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//public static readonly DependencyProperty TopLevelCategoriesProprerty =
		//    DependencyProperty.Register("TopLevelCategories", typeof(IEnu))

		public IEnumerable<Category> TopLevelCategories { get; private set; }

		public MainWindow()
		{
			SQLiteConnection conn = new SQLiteConnection("Data Source=..\\..\\..\\..\\..\\Data\\FCHA_Master;Version=3;");
			conn.Open();

			CategoriesManager mgr = new CategoriesManager(conn);
			TopLevelCategories = new List<Category>(mgr.EnumCategoriesByParent(0));

			InitializeComponent();
		}
	}
}
