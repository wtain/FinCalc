using FCHA.CBR;
using FCHA.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace FCHA
{
    public class CbrClient : DependencyObject, IFXRateSource
    {
        private DailyInfoSoapClient m_srv = new DailyInfoSoapClient();
        private Dictionary<string, FxRate> m_rates = new Dictionary<string, FxRate>();

        private object m_lock = new object();

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
            get { lock (m_lock) return (IEnumerable<FxRate>)GetValue(FXRatesProperty); }
            set { lock (m_lock) SetValue(FXRatesProperty, value); }
        }

        public CbrClient()
        {
            m_srv.GetCursOnDateCompleted += OnFxRatesRequestCompleted;
            SelectedDate = DateTime.Now.Date;
        }

        public FxRate GetFXRate(string CCY)
        {
            lock (m_lock)
            {
                if (null == m_rates || !m_rates.ContainsKey(CCY))
                    return null;
                return m_rates[CCY];
            }
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
            lock (m_lock)
            {
                FXRates = rates;
                m_rates = new Dictionary<string, FxRate>();
                foreach (FxRate rate in FXRates)
                    m_rates.Add(rate.CCY, rate);
            }
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CbrClient This = (CbrClient)d;
            This.m_srv.GetCursOnDateAsync((DateTime)e.NewValue);
        }
    }
}
