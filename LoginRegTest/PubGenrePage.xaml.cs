using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginRegTest
{
    public partial class PubGenrePage : Page
    {
        ShopEntities shopGames = new ShopEntities();
        public PubGenrePage()
        {
            InitializeComponent();
            Genre_Grid.ItemsSource = shopGames.Genres.ToList();
            Pub_Grid.ItemsSource = shopGames.Publisher.ToList();
        }


        private void Genre_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Genres)Genre_Grid.SelectedItem;
            if(selected != null)
            {
                InfoGenre_Tbl.Text = "";
                Genre_Tbx.Text = selected.GanreName;
            }

        }

        private void Pub_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (Publisher)Pub_Grid.SelectedItem;
            if (selected != null)
            {
                InfoGenre_Tbl.Text = "";
                Pub_Tbx.Text = selected.NamePublisher;
            }
        }

        private void ClearGenre_But_Click(object sender, RoutedEventArgs e)
        {
            Genre_Tbx.Clear();
        }

        private void ClearePub_But_Click(object sender, RoutedEventArgs e)
        {
            Pub_Tbx.Clear();
        }

        private void AddGenre_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            if (!string.IsNullOrWhiteSpace(Genre_Tbx.Text) && validation.IsOnlyBykvi(Genre_Tbx.Text))
            {
                var gen = shopGames.Genres.ToList().Where(i => i.GanreName == Genre_Tbx.Text).ToList();
                if (gen.Count() == 0)
                {
                    Genres genre = new Genres();
                    genre.GanreName = Genre_Tbx.Text;
                    shopGames.Genres.Add(genre);
                    shopGames.SaveChanges();
                    Genre_Grid.ItemsSource = shopGames.Genres.ToList();
                }
                else
                {
                    InfoGenre_Tbl.Text = "Такой жанр уже существует!";
                }
            }
            else
            {
                InfoGenre_Tbl.Text = "Проверьте ввод!";
            }
        }

        private void AddPub_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            if (!string.IsNullOrWhiteSpace(Pub_Tbx.Text) && validation.IsOnlyBykvi(Pub_Tbx.Text))
            {
                var pub = shopGames.Publisher.ToList().Where(i => i.NamePublisher == Pub_Tbx.Text).ToList();
                if (pub.Count() == 0)
                {
                    Publisher publisher = new Publisher();
                    publisher.NamePublisher = Pub_Tbx.Text;
                    shopGames.Publisher.Add(publisher);
                    shopGames.SaveChanges();
                    Pub_Grid.ItemsSource = shopGames.Publisher.ToList();
                }
                else
                {
                    InfoPub_Tbl.Text = "Такой издатель уже существует!";
                }
            }
            else
            {
                InfoPub_Tbl.Text = "Проверьте ввод!";
            }
        }

        private void UpdateGenre_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            var selected = (Genres)Genre_Grid.SelectedItem;
            if (selected != null)
            {
                if (!string.IsNullOrWhiteSpace(Genre_Tbx.Text) && validation.IsOnlyBykvi(Genre_Tbx.Text))
                {
                    var gen = shopGames.Genres.ToList().Where(i => i.GanreName == Genre_Tbx.Text).ToList();
                    if (gen.Count() == 0)
                    {
                        selected.GanreName = Genre_Tbx.Text;
                        shopGames.SaveChanges();
                        Genre_Grid.ItemsSource = shopGames.Genres.ToList();
                    }
                    else
                    {
                        InfoGenre_Tbl.Text = "Такой жанр уже существует!";
                    }
                }
                else
                {
                    InfoGenre_Tbl.Text = "Проверьте ввод!";
                }
            }
        }

        private void UpdatePub_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();
            var selected = (Publisher)Pub_Grid.SelectedItem;
            if (selected != null)
            {
                if (!string.IsNullOrWhiteSpace(Pub_Tbx.Text) && validation.IsOnlyBykvi(Pub_Tbx.Text))
                {
                    var pub = shopGames.Publisher.ToList().Where(i => i.NamePublisher == Pub_Tbx.Text).ToList();
                    if (pub.Count() == 0)
                    {
                        selected.NamePublisher = Pub_Tbx.Text;
                        shopGames.SaveChanges();
                        Pub_Grid.ItemsSource = shopGames.Publisher.ToList();
                    }
                    else
                    {
                        InfoPub_Tbl.Text = "Такой издатель уже существует!";
                    }
                }
                else
                {
                    InfoPub_Tbl.Text = "Проверьте ввод!";
                }
            }
        }

        private void DeleteGenre_But_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Genres)Genre_Grid.SelectedItem;
            if (selected != null)
            {
                var game = shopGames.Games.ToList().Where(i => i.genre_ID == selected.ID_Genre).ToList();
                if (game.Count() == 0)
                {
                    shopGames.Genres.Remove(selected);
                    shopGames.SaveChanges();
                    Genre_Grid.ItemsSource = shopGames.Genres.ToList();

                }
                else { InfoGenre_Tbl.Text = "Этот жанр привязан к игре!"; }
            }
        }

        private void DeletePub_But_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Publisher)Pub_Grid.SelectedItem;
            if(selected != null)
            {
                var game = shopGames.Games.ToList().Where(i => i.publisher_ID == selected.ID_Publisher).ToList();
                if(game.Count() == 0)
                {
                    shopGames.Publisher.Remove(selected);
                    shopGames.SaveChanges();
                    Pub_Grid.ItemsSource = shopGames.Publisher.ToList();

                }
                else { InfoPub_Tbl.Text = "Этот издатель привязан к игре!"; }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            List<Genres> genres = DeserData<Genres>("C:\\Users\\user\\Desktop\\Чеки\\Gen.json");
            List<Publisher> publishers = DeserData<Publisher>("C:\\Users\\user\\Desktop\\Чеки\\Pub.json");
            shopGames.Genres.AddRange(genres);
            shopGames.Publisher.AddRange(publishers);
            shopGames.SaveChanges();
            Genre_Grid.ItemsSource = shopGames.Genres.ToList();
            Pub_Grid.ItemsSource = shopGames.Publisher.ToList();
        }

        private static List<T> DeserData<T>(string path)
        {
            string fullpath = path;
            if (!File.Exists(fullpath))
            {

                string jsonContent = "[]";
                File.WriteAllText(fullpath, jsonContent);
            }
            string json = File.ReadAllText(fullpath);
            List<T> data = JsonConvert.DeserializeObject<List<T>>(json);
            return data;
        }
    }
}
