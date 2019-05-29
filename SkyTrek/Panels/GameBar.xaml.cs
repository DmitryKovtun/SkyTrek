using System.Windows.Controls;
using System.Windows.Media;

namespace SkyTrek.Panels
{
    /// <summary>
    /// Interaction logic for GameBar.xaml
    /// </summary>
    public partial class GameBar : UserControl
    {
        public GameBar()
        {
            InitializeComponent();
        }

        public void SetPlayerHealthIndicator(int value)
        {
            var f = value * 252 / 100;

            PlayerHealthIndicator.Width = f > 0 ? f : 0;

            if (f > 230)
               PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#8BC34A") as SolidColorBrush;
            if (f > 126 & f < 230)
                PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#F9AA33") as SolidColorBrush;
            else if (f <= 126)
                PlayerHealthIndicator.Background = new BrushConverter().ConvertFromString("#df4e56") as SolidColorBrush;
        }
    }
}
