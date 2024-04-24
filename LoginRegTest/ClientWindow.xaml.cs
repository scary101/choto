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
using System.Windows.Shapes;

namespace LoginRegTest
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void Profile_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new ProfilPage();
        }

        private void Magazin_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new ShopPage();
        }

        private void ShopCart_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new KorzinaPage();
        }

        private void History_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new ClOrderHisPage();
        }
    }
}
