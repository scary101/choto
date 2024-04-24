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
    /// Логика взаимодействия для BankCardWindow.xaml
    /// </summary>
    public partial class BankCardWindow : Window
    {
        private ShopEntities shopGames = new ShopEntities();
        private string loginuser = (string)App.Current.Resources["Login_User"];

        public BankCardWindow()
        {
            InitializeComponent();
            var accounts = shopGames.Accounts.FirstOrDefault(item => item.LoginAcc == loginuser);
            var user = shopGames.Users.FirstOrDefault(item => item.ID_User == accounts.users_ID);
            var numCard = shopGames.PaymentData.Where(item => item.UserID == user.ID_User).ToList();
            if (numCard.Count() != 0)
            {
                NomerCard.Text = numCard[0].CardNumber;
                CVV.Text = numCard[0].CVV;
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            var accounts = shopGames.Accounts.FirstOrDefault(item => item.LoginAcc == loginuser);
            var user = shopGames.Users.FirstOrDefault(item => item.ID_User == accounts.users_ID);
            var numCard = shopGames.PaymentData.Where(item => item.CardNumber == NomerCard.Text).ToList();
            var card = shopGames.PaymentData.Find(user.ID_User);
            bool checkDataCard = (validation.IsValidCardNumber(NomerCard.Text) && validation.IsValidCvvCard(CVV.Text) && numCard.Count() == 0) ? true: false;
            if (checkDataCard)
            {
                PaymentData payment = new PaymentData();
                payment.CardNumber = NomerCard.Text;
                payment.CVV = CVV.Text;
                payment.UserID = user.ID_User;
                
                if(card != null)
                {
                    shopGames.PaymentData.Remove(card);
                    shopGames.PaymentData.Add(payment);
                    shopGames.SaveChanges();

                }
                else { shopGames.PaymentData.Add(payment); shopGames.SaveChanges(); }
            }
            else
            {
                MessageBox.Show("Все хуйня, давай по новой");
            }

        }
    }
}
