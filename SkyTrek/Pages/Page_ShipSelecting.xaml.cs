using SkyTrekVisual.GameItems.Helpers;
using SkyTrekVisual.GameItems.StarShipList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Page_ShipSelecting.xaml
    /// </summary>
    public partial class Page_ShipSelecting : Page, INotifyPropertyChanged
    {
        public ObservableCollection<StarShip> StarShips { get; }

        private StarShip selectedShip;

        public StarShip SelectedShip
        {
            get { return selectedShip; }
            set { selectedShip = value; }
        }

        public event EventHandler Event_BackToMenu;
        public event EventHandler Event_StartNewGame;

        public Page_ShipSelecting()
        {
            InitializeComponent();

            StarShips = new ObservableCollection<StarShip>();

            if (TextureManager.Ship_previews.Length == 0)
                MessageBox.Show("Ships preview did not found!");
            else
            {
                foreach (var item in TextureManager.Ship_previews)
                {
                    StarShips.Add(new StarShip(item));
                }
            }

            DataContext = this;


			SelectedShip = StarShips[0];

			UserName = "Unknown player";
		}

        private void Button_BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Event_BackToMenu.Invoke(null, null);
        }

        private void Button_StartNewGame_Click(object sender, RoutedEventArgs e)
        {
            Event_StartNewGame.Invoke(SelectedShip, null);
        }


		private string userName;

		public string UserName
		{
			get { return userName; }
			set { userName = value; OnPropertyChanged("UserName"); }
		}




		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
