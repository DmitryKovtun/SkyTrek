using System;
using System.Windows;
using System.Windows.Controls;

namespace SkyTrek.Pages
{
    /// <summary>
    /// Interaction logic for Page_Creators.xaml
    /// </summary>
    public partial class Page_Creators : Page
    {
        public event EventHandler Event_BackToMenu;

        public Page_Creators()
        {
            InitializeComponent();
        }

        private void Button_BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Event_BackToMenu.Invoke(null, null);
        }
    }
}
