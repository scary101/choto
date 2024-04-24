using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginRegTest
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        ShopEntities shopGames = new ShopEntities();
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Reg_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            var allLogin = shopGames.Accounts.ToList().Where(item => item.LoginAcc == Login.Text).ToList();
            var allEmail = shopGames.Users.ToList().Where(item => item.Email == Email.Text).ToList();
            var allPhone = shopGames.Users.ToList().Where(item => item.PhoneNumber == Phone.Text).ToList();
            bool empty = !string.IsNullOrWhiteSpace(Login.Text) && !string.IsNullOrWhiteSpace(Password.Text) ? true : false;
            if(empty)
            {
                bool checkLogin = (validation.CheckCyrillicAndSpecialSymbol(Login.Text) && allLogin.Count() == 0) ? true : false;
                bool checkPassword = validation.CheckCyrillicAndSpecialSymbol(Password.Text);
                bool checkFio = validation.CheckNameSurMiddle(SNP.Text);
                bool checkEmail = (validation.CheckEmail(Email.Text) && allEmail.Count() == 0) ? true : false;
                bool checkPhone = (validation.CheckPhone(Phone.Text) && Phone.Text.Count() == 11 && allPhone.Count() == 0) ? true : false;
                if (checkLogin)
                {
                    if (checkPassword)
                    {
                        if (checkFio)
                        {
                            if (checkEmail)
                            {
                                if (checkPhone)
                                {
                                    Accounts account = new Accounts();
                                    Users user = new Users();
                                    string[] fio = SNP.Text.Split();
                                    user.Surname = fio[0];
                                    user.UserName = fio[1];
                                    user.MiddleName = fio[2];
                                    user.Email = Email.Text;
                                    user.PhoneNumber = Phone.Text;
                                    shopGames.Users.Add(user);
                                    shopGames.SaveChanges();
                                    account.LoginAcc = Login.Text;
                                    account.PasswordAcc = Password.Text;
                                    account.role_ID = 2;
                                    var userID = shopGames.Users.ToList().Where(item => item.PhoneNumber == Phone.Text).ToArray();
                                    account.users_ID = userID[0].ID_User;
                                    shopGames.Accounts.Add(account);
                                    shopGames.SaveChanges();
                                    ClearAllText();
                                }
                                else { InfoReg.Text = "Неккоректный ввод номера телефона или он уже занят!"; }
                            }
                            else { InfoReg.Text = "Неккоректный ввод  почты или она уже занята!"; }
                        }
                        else { InfoReg.Text = "Неккоректный ввод ФИО"; }
                    }
                    else { InfoReg.Text = "Неккоректный ввод пароля!"; }
                }
                else { InfoReg.Text = "Неккоректный ввод логина или такой уже существует!"; }
            }
            else
            {
                InfoReg.Text = "Поля не могут быть пустыми!";
            }

        }

       private void ClearAllText()
       {
            InfoReg.Text = "Аккаунт под именем " + Login.Text + " зарегестрирован! Теперь нажмите кнопку Войти";
            Login.Clear();
            Password.Clear();
            SNP.Clear();
            Email.Clear();
            Phone.Clear();
       }
    }
}
