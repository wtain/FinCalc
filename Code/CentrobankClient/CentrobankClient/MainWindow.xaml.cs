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
using CentrobankClient.ru.cbr.www;
using System.Data;

namespace CentrobankClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DailyInfo srv = new DailyInfo();

		public class FxRate
		{
			public string CCY { get; private set; }
			public decimal Rate { get; private set; }

			public FxRate(string ccy, decimal rate)
			{
				CCY = ccy;
				Rate = rate;
			}
		}

		public static readonly DependencyProperty FXRatesProperty =
			DependencyProperty.Register("FXRates", typeof(IEnumerable<FxRate>), typeof(MainWindow));

		public IEnumerable<FxRate> FXRates
		{
			get { return (IEnumerable<FxRate>) GetValue(FXRatesProperty); }
			set { SetValue(FXRatesProperty, value); }
		}

		public MainWindow()
		{
			InitializeComponent();
			dtPick.SelectedDate = DateTime.Now;
			
			//srv.GetCursDynamic(DateTime.Now,)
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!dtPick.SelectedDate.HasValue)
				return;
			DataSet fx = srv.GetCursOnDate(dtPick.SelectedDate.Value);
			DataTable fxrates = fx.Tables["ValuteCursOnDate"];
			List<FxRate> rates = new List<FxRate>();
			foreach (DataRow r in fxrates.Rows)
			{
				string Vname = r["Vname"] as string; Vname = Vname.Trim();
				decimal Vnom = (r["Vnom"] as decimal?).Value;
				decimal Vcurs = (r["Vcurs"] as decimal?).Value;
				int Vcode = (r["Vcode"] as int?).Value;
				string VchCode = r["VchCode"] as string;
				rates.Add(new FxRate(VchCode, Vcurs));
			}
			FXRates = rates;
		}
	}
}
