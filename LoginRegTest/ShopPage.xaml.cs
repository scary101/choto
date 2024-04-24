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
    
    public partial class ShopPage : Page
    {
        private ShopEntities shopGames = new ShopEntities();
        private List<Games> korzina = new List<Games>();
        public ShopPage()
        {
            InitializeComponent();


            List<Genres> genres = new List<Genres>();
            genres.Add(new Genres { GanreName = "Ничего", ID_Genre = -1 });
            genres.AddRange(shopGames.Genres.ToList());
            SortByGenre.ItemsSource = genres;

            List<Publisher> publishers = new List<Publisher>();
            publishers.Add(new Publisher { NamePublisher = "Ничего", ID_Publisher = -1 });
            publishers.AddRange(shopGames.Publisher.ToList());
            SortByPub.ItemsSource = publishers;

            Games_grid.ItemsSource = shopGames.Games.ToList();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = (Games)Games_grid.SelectedItem;
            if (select != null)
            {
                var sysreq = shopGames.SysRequirements.FirstOrDefault(i => i.GameID == select.ID_Game);
                if (sysreq != null)
                {
                    GameWho_Tbl.Text = "Системные требования: " + select.GameName;
                    Tbl_Proc.Text = "Процессор: " + sysreq.Processor;
                    Tbl_Video.Text = "Видеокарта: " + sysreq.VideoCard;
                    Tbl_RAM.Text = "RAM (GB): " + sysreq.DDR.ToString();
                    Tbl_Space.Text = "Место на диске: " + sysreq.DiskSpace.ToString();
                }
                else
                {
                    GameWho_Tbl.Text = "Системных требований пока нет";
                }
            }
        }

        private void FilterGames()
        {
            if (SortByGenre != null && SortByPub != null)
            {
                var selectedGenre = (Genres)SortByGenre.SelectedItem;
                var selectedPublisher = (Publisher)SortByPub.SelectedItem;
                var searchText = Search.Text.ToLower(); 

                var allGames = shopGames.Games.ToList();

                var filteredGames = allGames;

                if (selectedGenre != null && selectedGenre.ID_Genre != -1)
                {
                    filteredGames = filteredGames.Where(g => g.genre_ID == selectedGenre.ID_Genre).ToList();
                }

                if (selectedPublisher != null && selectedPublisher.ID_Publisher != -1)
                {
                    filteredGames = filteredGames.Where(g => g.publisher_ID == selectedPublisher.ID_Publisher).ToList();
                }

                filteredGames = filteredGames.Where(g => g.GameName.ToLower().Contains(searchText)).ToList();

                Games_grid.ItemsSource = filteredGames;
            }
        }

        private void SortByGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterGames();
        }

        private void SortByPub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterGames();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterGames();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Games)Games_grid.SelectedItem;
            if (selected != null)
            {
                var stor = shopGames.Storage.FirstOrDefault(i => i.ID_Storage == selected.ID_Game);
                if (stor != null && stor.Quantity > 0)
                {
                    stor.Quantity--;
                    App.Current.Resources["Korzina"] += " " + selected.ID_Game.ToString();
                    shopGames.SaveChanges();
                    Games_grid.ItemsSource = shopGames.Games.ToList();
                }
                else
                {
                    MessageBox.Show("Недостаточно товара на складе!");
                }
            }
        }
    }
}
