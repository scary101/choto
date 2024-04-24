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
    /// <summary>
    /// Логика взаимодействия для ClOrderHisPage.xaml
    /// </summary>
    public partial class ClOrderHisPage : Page
    {
        ShopEntities shopGames = new ShopEntities();
        public ClOrderHisPage()
        {
            InitializeComponent();
            string login = App.Current.Resources["Login_User"].ToString();
            var acc = shopGames.Accounts.FirstOrDefault(i => i.LoginAcc == login);
            var orders = shopGames.Orders.Where(i => i.account_id == acc.ID_Account).ToList();
            HisGrid.ItemsSource = orders;
        }
    }
}
