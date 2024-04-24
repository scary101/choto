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

    public partial class RoleWindow : Window
    {
        ShopEntities shopGames = new ShopEntities();
        public RoleWindow()
        {
            InitializeComponent();
            RoleGrid.ItemsSource = shopGames.Roles.ToList();
        }

        private void Save_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            if (!string.IsNullOrWhiteSpace(TbxRole.Text))
            {
                if (validation.IsOnlyBykvi(TbxRole.Text))
                {
                    if (!shopGames.Roles.Any(r => r.RoleName == TbxRole.Text))
                    {
                        Roles role = new Roles();
                        role.RoleName = TbxRole.Text;
                        shopGames.Roles.Add(role);
                        shopGames.SaveChanges();
                        RoleGrid.ItemsSource = shopGames.Roles.ToList();
                    }
                    else
                    {
                        TblRole.Text = "Роль уже существует!";
                    }
                }
                else
                {
                    TblRole.Text = "Не правильный ввод!";
                }
            }
            else
            {
                TblRole.Text = "Введите название роли!";
            }
        }

        private void Update_But_Click(object sender, RoutedEventArgs e)
        {
            var select = (Roles)RoleGrid.SelectedItem;
            if (select != null)
            {
                if (!string.IsNullOrWhiteSpace(TbxRole.Text))
                {
                    if (!shopGames.Roles.Any(r => r.RoleName == TbxRole.Text && r.ID_Role != select.ID_Role))
                    {
                        Roles role = new Roles();
                        role.RoleName = TbxRole.Text;

                        shopGames.Entry(select).CurrentValues.SetValues(role);
                        shopGames.SaveChanges();
                        RoleGrid.ItemsSource = shopGames.Roles.ToList();
                    }
                    else
                    {
                        TblRole.Text = "Роль уже существует!";
                    }
                }
                else
                {
                    TblRole.Text = "Введите название роли!";
                }
            }
            else
            {
                TblRole.Text = "Выберите роль для обновления!";
            }
        }

        private void Delete_But_Click(object sender, RoutedEventArgs e)
        {
            var select = (Roles)RoleGrid.SelectedItem;
            if (select != null)
            {
                var isUse = shopGames.Accounts.FirstOrDefault(i => i.role_ID == select.ID_Role);
                if(isUse != null)
                {
                    TblRole.Text = "Эта роль используется!";
                }
                else
                {
                    shopGames.Roles.Remove(select);
                    shopGames.SaveChanges();
                    RoleGrid.ItemsSource = shopGames.Roles.ToList();
                }
            }
        }

        private void RoleGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = (Roles)RoleGrid.SelectedItem;
            if (select != null)
            {
                TblRole.Text = "";
                TbxRole.Text = select.RoleName;
            }
        }

        private void Exit_But_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            Close();
        }
    }
}
