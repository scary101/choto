using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class DepositeWindow : Window
    {
        private ShopEntities shopGames = new ShopEntities();
        private List<Games> games = new List<Games>();
        private int final;
        public DepositeWindow(List<Games> games,int final)
        {
            InitializeComponent();
            this.games = games;
            this.final = final;
            FinPay_Tbl.Text = "К оплате: " + final.ToString();
        }

        private void Pay_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            if (validation.CheckValidNum(Pay_Tbx.Text))
            {
                if (Convert.ToInt32(Pay_Tbx.Text) >= final)
                {
                    string login = App.Current.Resources["Login_User"].ToString();
                    var acc = shopGames.Accounts.FirstOrDefault(i => i.LoginAcc == login);
                    Change_Tbl.Text = "Сдача: " + (Convert.ToInt32(Pay_Tbx.Text) - final).ToString();
                    Orders order = new Orders
                    {
                        DateOrder = DateTime.Today,
                        FinalyCoast = final,
                        Deposite = Convert.ToInt32(Pay_Tbx.Text),
                        ChangeDep = Convert.ToInt32(Pay_Tbx.Text) - final,
                        account_id = acc.ID_Account
                    };
                    shopGames.Orders.Add(order);
                    shopGames.SaveChanges();
                    int orderId = order.ID_Order;
                    foreach (var game in games)
                    {
                        Vouchers voucher = new Vouchers
                        {
                            game_id = game.ID_Game,
                            orders_id = orderId
                        };
                        shopGames.Vouchers.Add(voucher);
                    }
                    shopGames.SaveChanges();

                    MessageBox.Show("Заказ успешно сохранен.");
                    CreateVoucher(order);


                }
                else
                {
                    MessageBox.Show("Недостаточно средств!");
                }
            }
            else { MessageBox.Show("Введите число!"); }
            
        }

        private void CreateVoucher(Orders order)
        {
            string fileName = "C:\\Users\\user\\Desktop\\Чеки\\VoucherForOrder_" + order.ID_Order + ".txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Game Store");
                writer.WriteLine($"Кассовый чек №{order.ID_Order}\n");

                foreach (var item in games)
                {
                    writer.WriteLine($"{item.GameName}\t{item.Coast}");
                }

                writer.WriteLine($"\nИтого к оплате: {final}");
                writer.WriteLine($"Внесено: {Pay_Tbx.Text}");
                writer.WriteLine($"{Change_Tbl.Text}");
                writer.Close();
            }
            Close();
        }
    }
}
