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
using System.Windows.Shapes;

namespace FCHA
{
	/// <summary>
	/// Interaction logic for InputDialog.xaml
	/// </summary>
	public partial class InputDialog : Window
	{
		public static readonly DependencyProperty ValueProperty
			= DependencyProperty.Register("Value", typeof(string), typeof(InputDialog));

		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public InputDialog()
		{
			InitializeComponent();
			txtValue.Focus();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void dlgInputBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			txtValue.SelectAll();
		}
	}
}
