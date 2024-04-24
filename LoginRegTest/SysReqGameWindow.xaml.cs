using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class SysReqGameWindow : Window
    {
        private int GameID;
        private ShopEntities shopGames = new ShopEntities();
        public SysReqGameWindow(int gameID)
        {
            InitializeComponent();
            GameID = gameID;
            var game = shopGames.Games.FirstOrDefault(item => item.ID_Game == GameID);
            var sys = shopGames.SysRequirements.FirstOrDefault(item => item.GameID == GameID);
            if(sys != null)
            {
                Processor_Tbx.Text = sys.Processor;
                VideoCard_Tbx.Text = sys.VideoCard;
                RAM_Tbx.Text = sys.DDR.ToString();
                DisckSpace_Tbx.Text = sys.DiskSpace.ToString();
            }

            TitleGame.Text = "Системные требования для игры " + game.GameName;
        }

        private void Save_But_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();

            bool checkAll = (!string.IsNullOrWhiteSpace(Processor_Tbx.Text) &&
                             !string.IsNullOrWhiteSpace(VideoCard_Tbx.Text) &&
                             validation.CheckCyrillicAndSpecialSymbol(Processor_Tbx.Text.Trim()) &&
                             validation.CheckCyrillicAndSpecialSymbol(VideoCard_Tbx.Text.Trim()) &&
                             validation.CheckValidNum(RAM_Tbx.Text) &&
                             validation.CheckValidNum(DisckSpace_Tbx.Text) &&
                             Convert.ToInt32(RAM_Tbx.Text) > 0 &&
                             Convert.ToInt32(DisckSpace_Tbx.Text) > 0);

            var req = shopGames.SysRequirements.Find(GameID);
            if (checkAll)
            {
                SysRequirements requirements = new SysRequirements();
                requirements.GameID = GameID;
                requirements.Processor = Processor_Tbx.Text;
                requirements.VideoCard = VideoCard_Tbx.Text;
                requirements.DDR = Convert.ToInt32(RAM_Tbx.Text);
                requirements.DiskSpace = Convert.ToInt32(DisckSpace_Tbx.Text);
                if (req != null)
                {
                    shopGames.SysRequirements.Remove(req);
                    shopGames.SysRequirements.Add(requirements);
                    shopGames.SaveChanges();
                }
                else
                {
                    shopGames.SysRequirements.Add(requirements);
                    shopGames.SaveChanges();
                }
                Info_Txb.Text = "Данные успешно добавлены!";
            }
            else
            {
                Info_Txb.Text = "Ошибка! Проверьте правильность ввода!";
            }
        }

    }
}
