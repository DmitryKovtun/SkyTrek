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

namespace SkyTrek.Pages
{
    /// <summary>
    /// Interaction logic for Page_Menu.xaml
    /// </summary>
    public partial class Page_Menu : Page
    {
        public event EventHandler Event_NewGame;
        public event EventHandler Event_ContinueGame;
        public event EventHandler Event_Settings;
        public event EventHandler Event_Creators;
        public event EventHandler Event_Exit;
        public event EventHandler Event_Score;

        public Page_Menu()
        {
            InitializeComponent();

            Set_IsEnabled_Button_Continue(false);
        }

        private void Button_Creators_Click(object sender, RoutedEventArgs e)
        {
            Event_Creators.Invoke(null, null);
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            Event_Settings.Invoke(null, null);
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Event_Exit.Invoke(null, null);
        }

        private void Button_NewGame_Click(object sender, RoutedEventArgs e)
        {
            Event_NewGame.Invoke(null, null);
        }

        private void Button_Score_Click(object sender, RoutedEventArgs e)
        {
            Event_Score.Invoke(null, null);
        }

        private void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            Event_ContinueGame.Invoke(null, null);
        }

        public void Set_IsEnabled_Button_Continue(bool value)
        {
            Button_Continue.IsEnabled = value;
        }

    }
}
