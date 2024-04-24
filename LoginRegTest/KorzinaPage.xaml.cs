using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class KorzinaPage : Page
    {
        ShopEntities shopGames = new ShopEntities();
        private int coastfin;
        public KorzinaPage()
        {
            InitializeComponent();
            RefreshKorzina();
        }


        private void RefreshKorzina()
        {
            int fin = 0;
            try
            {
                string korzina = App.Current.Resources["Korzina"].ToString();
                string[] id = korzina.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<Games> games = new List<Games>();
                foreach (string num in id)
                {
                    int gameId = Convert.ToInt32(num);
                    Games game = shopGames.Games.Include("Genres").Include("Publisher").FirstOrDefault(g => g.ID_Game == gameId);
                    fin += game.Coast;
                    games.Add(game);
                }
                Finaly.Text = fin.ToString();
                coastfin = fin;
                GameInKorzina.ItemsSource = games;
            }catch (Exception ex)
            {
                MessageBox.Show("Корзина пока пуста(");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var game = (Games)GameInKorzina.SelectedItem;
            if (game != null)
            {
                string korzina = App.Current.Resources["Korzina"].ToString();

                string[] id = korzina.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int indexToRemove = Array.IndexOf(id, game.ID_Game.ToString());
                if (indexToRemove != -1)
                {
                    List<string> updatedIdList = id.ToList();
                    updatedIdList.RemoveAt(indexToRemove);

                    string updatedKorzina = string.Join(" ", updatedIdList);

                    App.Current.Resources["Korzina"] = updatedKorzina;

                    RefreshKorzina();
                }
            }
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (GameInKorzina.ItemsSource != null)
            {
                var games = GameInKorzina.ItemsSource.Cast<Games>().ToList();
                App.Current.Resources["Korzina"] = "";
                DepositeWindow deposite = new DepositeWindow(games, coastfin);
                deposite.Show();
            }
            else
            {
                MessageBox.Show("Ошибка! Корзина пустая");
            }
        }
    }
}
