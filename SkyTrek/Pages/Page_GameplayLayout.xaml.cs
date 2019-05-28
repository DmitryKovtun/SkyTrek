using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyTrek.Pages
{
    /// <summary>
    /// Interaction logic for Page_GameplayLayout.xaml
    /// </summary>
    public partial class Page_GameplayLayout : Page
    {
        public Page_GameplayLayout()
        {
            InitializeComponent();
        }

        public bool SetPause()
        {
            layoutManager.IsPause = !layoutManager.IsPause;

            return layoutManager.IsPause;
        }

        public void SetGameOver()
        {
            layoutManager.IsGameOver = !layoutManager.IsGameOver;
        }


    }
}
