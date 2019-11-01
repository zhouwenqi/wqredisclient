using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wqredisclient.components
{
    /// <summary>
    /// SearchBox control
    /// </summary>
    public partial class SearchKeyBox : UserControl
    {
        public SearchKeyBox()
        {
            InitializeComponent();
        }

        private void inputKeyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputKeyBox.Text.Trim()))
            {
                btnRemove.Visibility = Visibility.Collapsed;
                icoSearch.Visibility = Visibility.Visible;
                inputLabel.Visibility = Visibility.Visible;
            }
            else
            {
                btnRemove.Visibility = Visibility.Visible;
                icoSearch.Visibility = Visibility.Collapsed;
                inputLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            inputKeyBox.Text = "";
        }

        public TextBox Input
        {
            get
            {
                return inputKeyBox;
            }
        }
    }
}
