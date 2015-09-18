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

namespace FCHA
{
	/// <summary>
	/// Interaction logic for OlapView.xaml
	/// </summary>
	public partial class OlapView : UserControl
	{
		public static readonly DependencyProperty OlapViewPanelProperty =
			DependencyProperty.Register("OlapViewPanel", typeof(Panel), typeof(OlapView));

		public Panel OlapViewPanel
		{
			get { return (Panel)GetValue(OlapViewPanelProperty); }
			set { SetValue(OlapViewPanelProperty, value); }
		}

		public OlapView(SQLiteConnection conn, string tableName, OlapStage stage)
		{
			OlapViewPanel = OlapBuilder.BuildOlapView(conn, tableName, stage);
			InitializeComponent();
		}
	}
}
