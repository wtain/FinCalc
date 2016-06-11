using FCHA.DataTypes;
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

namespace FCHA.Dialogs
{
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : Window
    {
        public static readonly DependencyProperty CategoryNameProperty
            = DependencyProperty.Register("CategoryName", typeof(string), typeof(CategoryDialog));

        public static readonly DependencyProperty TypeProperty
            = DependencyProperty.Register("Type", typeof(CategoryType), typeof(CategoryDialog));

        public static Array CategoryTypes
        {
            get { return Enum.GetValues(typeof(CategoryType)); }
        }

        public string CategoryName
        {
            get { return (string)GetValue(CategoryNameProperty); }
            set { SetValue(CategoryNameProperty, value); }
        }

        public CategoryType Type
        {
            get { return (CategoryType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public CategoryDialog()
        {
            InitializeComponent();
            txtCategoryName.Focus();
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

        private void dlgCategory_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            txtCategoryName.SelectAll();
        }
    }
}
