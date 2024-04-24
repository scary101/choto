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
    public partial class GameAdminPage : Page
    {
        private ShopEntities shopGames = new ShopEntities();
        public GameAdminPage()
        {
            InitializeComponent();
            GameRoster_Grid.ItemsSource = shopGames.Games.ToList();
            Genre_Cmbx.ItemsSource = shopGames.Genres.ToList();
            Publisher_Cmbx.ItemsSource = shopGames.Publisher.ToList();
        }

        private void GameRoster_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGame = (Games)GameRoster_Grid.SelectedItem;
            if (selectedGame != null)
            {
                var stor = shopGames.Storage.FirstOrDefault(i => i.ID_Storage == selectedGame.ID_Game);
                Info_Tbl.Text = "";
                Quant_Tbx.Text = stor.Quantity.ToString();
                NameGame_Tbx.Text = selectedGame.GameName;
                Coast_Tbx.Text = selectedGame.Coast.ToString();
                Genre_Cmbx.SelectedItem = selectedGame.Genres;
                Publisher_Cmbx.SelectedItem = selectedGame.Publisher;
            }
        }


        private void Clear_But_Click(object sender, RoutedEventArgs e)
        {
            Info_Tbl.Text = "";
            Quant_Tbx.Clear();
            NameGame_Tbx.Clear();
            Coast_Tbx.Clear();
            Genre_Cmbx.SelectedItem = null;
            Publisher_Cmbx.SelectedItem = null;
        }
        private void Add_But_Click(object sender, RoutedEventArgs e)
        {
            bool emtpty = (!string.IsNullOrWhiteSpace(NameGame_Tbx.Text) && !string.IsNullOrWhiteSpace(Coast_Tbx.Text) && !string.IsNullOrWhiteSpace(Quant_Tbx.Text) && Genre_Cmbx.SelectedItem != null && Publisher_Cmbx.SelectedItem != null) ? true : false;
            Validation validation = new Validation();
            bool notNull = (NameGame_Tbx.Text != null && validation.CheckValidNum(Coast_Tbx.Text) && Coast_Tbx.Text != null && Genre_Cmbx.SelectedItem != null && Publisher_Cmbx != null && validation.CheckValidNum(Quant_Tbx.Text) && Convert.ToInt32(Quant_Tbx.Text) > 0) ? true : false;
            if (notNull && !emtpty)
            {
                Storage storage = new Storage();
                Games game = new Games();
                game.GameName = NameGame_Tbx.Text;
                game.Coast = Convert.ToInt32(Coast_Tbx.Text);
                game.genre_ID = (int)Genre_Cmbx.SelectedValue;
                game.publisher_ID = (int)Publisher_Cmbx.SelectedValue;
                shopGames.Games.Add(game);
                shopGames.SaveChanges();
                storage.ID_Storage = game.ID_Game;
                storage.Quantity = Convert.ToInt32(Quant_Tbx.Text);
                shopGames.Storage.Add(storage);
                shopGames.SaveChanges();
                GameRoster_Grid.ItemsSource = shopGames.Games.ToList();
            }
            else { Info_Tbl.Text = "Неправильный ввод значений!"; }
        }

        private void Delete_But_Click(object sender, RoutedEventArgs e)
        {
            if (GameRoster_Grid.SelectedItem != null)
            {
                Games selectedGame = (Games)GameRoster_Grid.SelectedItem;
                Storage storage = shopGames.Storage.FirstOrDefault(s => s.ID_Storage == selectedGame.ID_Game);

                if (storage != null)
                {
                    shopGames.Storage.Remove(storage);
                }
                shopGames.Games.Remove(selectedGame);
                shopGames.SaveChanges();
                GameRoster_Grid.ItemsSource = shopGames.Games.ToList();
            }
        }


        private void ReqGame_But_Click(object sender, RoutedEventArgs e)
        {
            Games selectedGame = (Games)GameRoster_Grid.SelectedItem;
            if (selectedGame != null)
            {
                SysReqGameWindow requirements = new SysReqGameWindow(selectedGame.ID_Game);
                requirements.Show();
            }
        }

        private void Update_But_Click(object sender, RoutedEventArgs e)
        {
            bool empty = (!string.IsNullOrWhiteSpace(NameGame_Tbx.Text) && !string.IsNullOrWhiteSpace(Coast_Tbx.Text) && !string.IsNullOrWhiteSpace(Quant_Tbx.Text) && Genre_Cmbx.SelectedItem != null && Publisher_Cmbx.SelectedItem != null) ? true : false;
            if (empty)
            {
                Validation validation = new Validation();
                bool notNull = (NameGame_Tbx.Text != null && validation.CheckValidNum(Coast_Tbx.Text) && Coast_Tbx.Text != null && Genre_Cmbx.SelectedItem != null && Publisher_Cmbx != null && Convert.ToInt32(Quant_Tbx.Text) > 0 && validation.CheckValidNum(Quant_Tbx.Text)) ? true : false;
                Games selectedGame = (Games)GameRoster_Grid.SelectedItem;
                if (selectedGame != null && notNull && empty)
                {
                    Storage storage = shopGames.Storage.FirstOrDefault(s => s.ID_Storage == selectedGame.ID_Game);
                    if (storage != null)
                    {
                        storage.Quantity = Convert.ToInt32(Quant_Tbx.Text);

                        shopGames.SaveChanges();
                    }
                    else
                    {
                        Info_Tbl.Text = "Для этой игры отсутствует информация о складе!";
                    }
                    Games game = new Games();
                    game.ID_Game = selectedGame.ID_Game;
                    game.GameName = NameGame_Tbx.Text;
                    game.Coast = Convert.ToInt32(Coast_Tbx.Text);
                    game.genre_ID = (int)Genre_Cmbx.SelectedValue;
                    game.publisher_ID = (int)Publisher_Cmbx.SelectedValue;
                    shopGames.Entry(selectedGame).CurrentValues.SetValues(game);
                    shopGames.SaveChanges();

                    GameRoster_Grid.ItemsSource = shopGames.Games.ToList();
                }
                else { Info_Tbl.Text = "Неправильный ввод значений!"; }
            }
            else
            {
                MessageBox.Show("Поля не могут быть пустыми!");
            }
        }
    }
}
