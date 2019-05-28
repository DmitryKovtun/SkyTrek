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
    /// Interaction logic for Page_Settings.xaml
    /// </summary>
    public partial class Page_Settings : Page
    {
        public event EventHandler Event_BackToMenu;

        public Page_Settings()
        {
            InitializeComponent();
        }

        private void Button_BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Event_BackToMenu.Invoke(null, null);
        }
    }
}
