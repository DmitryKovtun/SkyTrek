using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using SkyTrekVisual.GameItems.ScoreItemList;

namespace SkyTrek.Pages
{
    /// <summary>
    /// Interaction logic for Page_Score.xaml
    /// </summary>
    public partial class Page_Score : Page
    {
        public event EventHandler Event_BackToMenu;




		public Page_Score()
        {
            InitializeComponent();

        }

        private void Button_BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Event_BackToMenu.Invoke(null, null);
        }





    }
}
