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
		private SQLiteConnection m_conn;
		private string m_tableName;
		private OlapStage m_stage;
        private string m_name;

		public static readonly DependencyProperty OlapViewPanelProperty =
			DependencyProperty.Register("OlapViewPanel", typeof(Panel), typeof(OlapView));

		public Panel OlapViewPanel
		{
			get { return (Panel)GetValue(OlapViewPanelProperty); }
			set { SetValue(OlapViewPanelProperty, value); }
		}

		public OlapView(string name, SQLiteConnection conn, string tableName, OlapStage stage)
		{
			m_conn = conn;
			m_tableName = tableName;
			m_stage = stage;
            m_name = name;
			RefreshView();
			InitializeComponent();
		}

		public void RefreshView()
		{
			OlapViewPanel = OlapBuilder.BuildOlapView(m_conn, m_tableName, m_stage);
		}

        // todo: introduce report, StageName -> ReportName

        public string StageName { get { return m_name; } }

        public override string ToString()
        {
            return StageName;
        }
    }
}
