using System.Windows.Media.Imaging;

namespace SkyTrekVisual.GameItems.StarShipList
{
	public class StarShip: NotifyPropertyChanged
    {
        private BitmapSource shipPreview;

        public BitmapSource ShipPreview
        {
            get { return shipPreview; }
            set { shipPreview = value; OnPropertyChanged("ShipPreview"); }
        }

        public StarShip()
        {

        }

        public StarShip(BitmapSource shipPreview) : this()
        {
            ShipPreview = shipPreview;
        }




    }
}
