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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Games_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new GameAdminPage();
        }

        private void GenPub_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new PubGenrePage();
        }

        private void Users_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new UsersPage();
        }

        private void Exit_But_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["Login_User"] = "";
            App.Current.Resources["Korzina"] = "";
            var old = Application.Current.MainWindow;
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            old.Close();
        }

        private void Orders_But_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new OrdersPage();
        }
    }
}
