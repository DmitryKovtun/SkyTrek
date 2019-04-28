using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace SkyTrekVisual.Tools
{
	public static class DictionaryAssistant
	{
		public enum Mode
		{
			Language,
			Theme,
			Accent
		}

		public static void SetResourceDictionary(string name, Mode mode)
		{
			if(!string.IsNullOrEmpty(name))
			{
				if(FindOldDictionary(mode) != null)
				{
					int index = Application.Current.Resources.MergedDictionaries.IndexOf(FindOldDictionary(mode));
					Application.Current.Resources.MergedDictionaries.Remove(FindOldDictionary(mode));
					Application.Current.Resources.MergedDictionaries.Insert(index, GetResourceDictionary(name, mode));
				}
				else
				{
					Application.Current.Resources.MergedDictionaries.Add(GetResourceDictionary(name, mode));
				}
			}
		}

		public static ResourceDictionary FindOldDictionary(Mode mode)
		{
			return (from d in Application.Current.Resources.MergedDictionaries
					where d.Source != null && d.Source.OriginalString.Contains(mode.ToString().ToLower())
					select d).FirstOrDefault() as ResourceDictionary;
		}

		public static ResourceDictionary GetResourceDictionary(string name, Mode mode)
		{
			if(!string.IsNullOrEmpty(name))
			{
				ResourceDictionary dictionary = new ResourceDictionary
				{
					//MessageBox.Show(Directory.GetCurrentDirectory().ToString());
					Source = new Uri(string.Format(@"/SkyTrekVisual;component/Resources/" + mode.ToString() + "s/" + mode.ToString().ToLower() + ".{0}.xaml", name), UriKind.Relative)
				};
				return dictionary;
			}
			return null;
		}

	}

}
