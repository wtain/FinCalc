using FCHA.CBR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace FCHA
{
    public class CbrClient : DependencyObject
    {
        private DailyInfoSoapClient m_srv = new DailyInfoSoapClient();

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(CbrClient), 
                new PropertyMetadata(OnSelectedDateChanged));

        public static readonly DependencyProperty FXRatesProperty =
            DependencyProperty.Register("FXRates", typeof(IEnumerable<FxRate>), typeof(CbrClient));

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public IEnumerable<FxRate> FXRates
        {
            get { return (IEnumerable<FxRate>)GetValue(FXRatesProperty); }
            set { SetValue(FXRatesProperty, value); }
        }

        public CbrClient()
        {
            m_srv.GetCursOnDateCompleted += OnFxRatesRequestCompleted;
            SelectedDate = DateTime.Now.Date;
        }

        private void OnFxRatesRequestCompleted(object sendser, GetCursOnDateCompletedEventArgs e)
        {
            DataTable fxrates = e.Result.Tables["ValuteCursOnDate"];
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

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CbrClient This = (CbrClient)d;
            This.m_srv.GetCursOnDateAsync((DateTime)e.NewValue);
        }
    }
}
