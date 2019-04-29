using System.IO;

namespace SkyTrek
{
	public class MainWindowViewModel
	{
		public static string CurrentDirectory { private set; get; }


		public MainWindowViewModel()
		{
			CurrentDirectory = Directory.GetCurrentDirectory();
		}




	}
}
