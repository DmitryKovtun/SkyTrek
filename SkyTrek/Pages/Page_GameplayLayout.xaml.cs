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



		public bool IsPause
		{
			get { return layoutManager.IsPause; }
			set { layoutManager.IsPause = value; }
		}

		public bool IsGameOver
		{
			get { return layoutManager.IsGameOver; }
			set { layoutManager.IsGameOver = value; }
		}


	



    }
}
