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

namespace LoginRegTest
{
    public partial class ProfilPage : Page
    {
        private ShopEntities shopGames = new ShopEntities();
        private string loginuser = (string)App.Current.Resources["Login_User"];
        public ProfilPage()
        {
            InitializeComponent();
            DataAcc_Grid.ItemsSource = null;
            DataAcc_Grid.Items.Clear();
            DataAcc_Grid.ItemsSource = shopGames.Accounts.ToList().Where(item => item.LoginAcc == loginuser);
        }

        private void DataCard_But_Click(object sender, RoutedEventArgs e)
        {
            BankCardWindow bankCard = new BankCardWindow();
            bankCard.Show();
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
    }
}
