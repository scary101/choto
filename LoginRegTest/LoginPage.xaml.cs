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
    public partial class LoginPage : Page
    {
        ShopEntities context = new ShopEntities();
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Load_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            bool good = false;
            var logpas = context.Accounts.ToList();
            string login = Login.Text;
            string password = Password.Password;
            int role = -1;
            bool checkLogPas = (validation.CheckCyrillicAndSpecialSymbol(Login.Text) && validation.CheckCyrillicAndSpecialSymbol(Password.Password)) ? true : false;

            if (!checkLogPas) { InfoMesLog.Text = "Неккоректный ввод данных! Проверьте язык раскладки!"; }

            for (int i = 0; i < logpas.Count(); i++)
            {
                if (logpas[i].LoginAcc == login && logpas[i].PasswordAcc == password)
                {
                    role = logpas[i].role_ID;
                    good = true;
                }
            }
            if (good)
            {
                if (role == 1)
                {
                    var old = Application.Current.MainWindow;
                    Application.Current.MainWindow = new AdminWindow();
                    Application.Current.MainWindow.Show();
                    old.Close();
                }
                else if (role == 2)
                {
                    App.Current.Resources["Login_User"] = Login.Text;
                    var old = Application.Current.MainWindow;
                    Application.Current.MainWindow = new ClientWindow();
                    Application.Current.MainWindow.Show();
                    old.Close();
                }
            }
            else
            {
                if (!checkLogPas) { InfoMesLog.Text = "Неккоректный ввод данных! Проверьте язык раскладки!"; }
                else { InfoMesLog.Text = "Неверный логин или пароль!"; }
            }
        }
    }
}
