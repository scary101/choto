using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Policy;
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
    public partial class UsersPage : Page
    {
        ShopEntities  shopGames = new ShopEntities();
        public UsersPage()
        {
            InitializeComponent();
            Users_Grid.ItemsSource = shopGames.Accounts.ToList();
            Role_Cbx.ItemsSource = shopGames.Roles.ToList();
        }

        private void Users_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = (Accounts)Users_Grid.SelectedItem;
            if (select != null)
            {
                var user = shopGames.Users.FirstOrDefault(i => i.ID_User == select.users_ID);
                Info_Tbl.Text = "";
                FIO_Tbx.Text = user.Surname + " " + user.UserName + " " + user.MiddleName;
                Email_Tbx.Text = user.Email;
                Phone_Tbx.Text = user.PhoneNumber;
                Login_Tbx.Text = select.LoginAcc;
                Pass_Tbx.Text = select.PasswordAcc;
                Role_Cbx.SelectedItem = select.Roles;
            }
        }

        private void Clear_But_Click(object sender, RoutedEventArgs e)
        {
            Info_Tbl.Text = "";
            FIO_Tbx.Text = "ФИО";
            Email_Tbx.Text = "Почта";
            Phone_Tbx.Text = "Телефон";
            Login_Tbx.Text = "Логин";
            Pass_Tbx.Text = "Пароль";
            Role_Cbx.SelectedItem = null;
        }

        private void Add_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            var allLogin = shopGames.Accounts.ToList().FirstOrDefault(item => item.LoginAcc == Login_Tbx.Text);
            var allEmail = shopGames.Users.ToList().FirstOrDefault(item => item.Email == Email_Tbx.Text);
            var allPhone = shopGames.Users.ToList().FirstOrDefault(item => item.PhoneNumber == Phone_Tbx.Text);

            bool emtpty = !string.IsNullOrWhiteSpace(Login_Tbx.Text) && !string.IsNullOrWhiteSpace(Pass_Tbx.Text) && !string.IsNullOrWhiteSpace(FIO_Tbx.Text) && !string.IsNullOrWhiteSpace(Email_Tbx.Text) && !string.IsNullOrWhiteSpace(Phone_Tbx.Text) ? true : false;

            bool checkLogin = (validation.CheckCyrillicAndSpecialSymbol(Login_Tbx.Text) && allLogin == null ) ? true : false;
            bool checkPassword = validation.CheckCyrillicAndSpecialSymbol(Pass_Tbx.Text);
            bool checkFio = validation.CheckNameSurMiddle(FIO_Tbx.Text);
            bool checkEmail = (validation.CheckEmail(Email_Tbx.Text) && allEmail == null ) ? true : false;
            bool checkPhone = (validation.CheckPhone(Phone_Tbx.Text) && Phone_Tbx.Text.Count() == 11 && allPhone == null) ? true : false;
            if (emtpty)
            {
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
                                    string[] fio = FIO_Tbx.Text.Split();
                                    user.Surname = fio[0];
                                    user.UserName = fio[1];
                                    user.MiddleName = fio[2];
                                    user.Email = Email_Tbx.Text;
                                    user.PhoneNumber = Phone_Tbx.Text;
                                    shopGames.Users.Add(user);
                                    shopGames.SaveChanges();
                                    account.LoginAcc = Login_Tbx.Text;
                                    account.PasswordAcc = Pass_Tbx.Text;
                                    account.role_ID = (int)Role_Cbx.SelectedValue;
                                    var userID = shopGames.Users.ToList().Where(item => item.PhoneNumber == Phone_Tbx.Text).ToArray();
                                    account.users_ID = userID[0].ID_User;
                                    shopGames.Accounts.Add(account);
                                    shopGames.SaveChanges();
                                    Users_Grid.ItemsSource = shopGames.Accounts.ToList();
                                }
                                else { Info_Tbl.Text = "Неккоректный ввод номера телефона или он уже занят!"; }
                            }
                            else { Info_Tbl.Text = "Неккоректный ввод  почты или она уже занята!"; }
                        }
                        else { Info_Tbl.Text = "Неккоректный ввод ФИО"; }
                    }
                    else { Info_Tbl.Text = "Неккоректный ввод пароля!"; }
                }
                else { Info_Tbl.Text = "Неккоректный ввод логина или такой уже существует!"; }
            }
        }

        private void Update_But_Click(object sender, RoutedEventArgs e)
        {
            Accounts select = (Accounts)Users_Grid.SelectedItem;

            bool emtpty = !string.IsNullOrWhiteSpace(Login_Tbx.Text) && !string.IsNullOrWhiteSpace(Pass_Tbx.Text) && !string.IsNullOrWhiteSpace(FIO_Tbx.Text) && !string.IsNullOrWhiteSpace(Email_Tbx.Text) && !string.IsNullOrWhiteSpace(Phone_Tbx.Text) ? true : false;

            if (select != null && emtpty)
            {
                Validation validation = new Validation();
                var olduser = shopGames.Users.FirstOrDefault(i => i.ID_User == select.users_ID);

                bool checkLogin = validation.CheckCyrillicAndSpecialSymbol(Login_Tbx.Text);
                bool checkPassword = validation.CheckCyrillicAndSpecialSymbol(Pass_Tbx.Text);
                bool checkFio = validation.CheckNameSurMiddle(FIO_Tbx.Text);
                bool checkEmail = validation.CheckEmail(Email_Tbx.Text);
                bool checkPhone = validation.CheckPhone(Phone_Tbx.Text) && Phone_Tbx.Text.Length == 11;

                if (checkLogin && checkPassword && checkFio && checkEmail && checkPhone)
                {
                    bool isLoginUnique = !shopGames.Accounts.Any(a => a.LoginAcc == Login_Tbx.Text && a.ID_Account != select.ID_Account);
                    bool isEmailUnique = !shopGames.Users.Any(u => u.Email == Email_Tbx.Text && u.ID_User != select.users_ID);
                    bool isPhoneUnique = !shopGames.Users.Any(u => u.PhoneNumber == Phone_Tbx.Text && u.ID_User != select.users_ID);

                    if (olduser.Email == Email_Tbx.Text) isEmailUnique = true;
                    if (olduser.PhoneNumber == Phone_Tbx.Text) isPhoneUnique = true;
                    if (select.LoginAcc == Login_Tbx.Text) isLoginUnique = true;

                    if (isLoginUnique && isEmailUnique && isPhoneUnique)
                    {
                        Accounts account = shopGames.Accounts.FirstOrDefault(a => a.ID_Account == select.ID_Account);
                        Users user = shopGames.Users.FirstOrDefault(u => u.ID_User == select.users_ID);

                        if (account != null && user != null)
                        {
                            string[] fio = FIO_Tbx.Text.Split();
                            user.Surname = fio[0];
                            user.UserName = fio[1];
                            user.MiddleName = fio[2];
                            user.Email = Email_Tbx.Text;
                            user.PhoneNumber = Phone_Tbx.Text;

                            account.LoginAcc = Login_Tbx.Text;
                            account.PasswordAcc = Pass_Tbx.Text;
                            account.role_ID = (int)Role_Cbx.SelectedValue;

                            shopGames.SaveChanges();
                            Users_Grid.ItemsSource = shopGames.Accounts.ToList();
                        }
                        else
                        {
                            Info_Tbl.Text = "Не удалось найти запись пользователя или аккаунта для обновления.";
                        }
                    }
                    else
                    {
                        if (!isLoginUnique) Info_Tbl.Text = "Логин уже занят!";
                        else if (!isEmailUnique) Info_Tbl.Text = "Email уже занят!";
                        else if (!isPhoneUnique) Info_Tbl.Text = "Номер телефона уже занят!";
                    }
                }
                else
                {
                    Info_Tbl.Text = "Некорректные введенные данные!";
                }
            }
        }

        private void Delete_But_Click(object sender, RoutedEventArgs e)
        {
            var select = (Accounts)Users_Grid.SelectedItem;
            if(select != null)
            {
                var user = shopGames.Users.FirstOrDefault(i => i.ID_User == select.users_ID);
                var card = shopGames.PaymentData.FirstOrDefault(i => i.UserID == user.ID_User);

                shopGames.Accounts.Remove(select);
                shopGames.Users.Remove(user);
                if(card != null)
                {
                    shopGames.PaymentData.Remove(card);
                }
                shopGames.SaveChanges();
                Users_Grid.ItemsSource = shopGames.Accounts.ToList();
            }
        }

        private void RoleTable_Click(object sender, RoutedEventArgs e)
        {
            var old = Application.Current.MainWindow;
            Application.Current.MainWindow = new RoleWindow();
            Application.Current.MainWindow.Show();
            old.Close();
        }

        private void Login_Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
