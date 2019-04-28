using System.Windows;
using System.Windows.Controls;

namespace SkyTrekVisual.Tools
{
	public static class PersonalizationManager
	{
		#region Language

		public static string[] ListOfLanguages
		{
			get { return new string[] { "Русский", "Українська", "English" }; }
			set { ListOfLanguages = value; }
		}

		private static string[] ListOfRootLanguages => new string[] { "Russian", "Ukrainian", "English" };

		//private static string DetermineCurrentLanguage()
		//{
		//	if(CultureInfo.CurrentCulture.Name.Contains("RU"))
		//	{
		//		return ListOfLanguages[0];
		//	}
		//	else if(CultureInfo.CurrentCulture.Name.Contains("UA"))
		//	{
		//		return ListOfLanguages[1];
		//	}
		//	else
		//	{
		//		return ListOfLanguages[2];
		//	}
		//}

		public static readonly DependencyProperty LanguageProperty = DependencyProperty.RegisterAttached("Language", typeof(string), typeof(PersonalizationManager), new PropertyMetadata("English", OnLanguageChanged, OnLanguageCoerceValue));

		//public static string GetLanguage(DependencyObject d) => (string)d.GetValue(LanguageProperty);

		//public static void SetLanguage(DependencyObject d, string value) => d.SetValue(LanguageProperty, value);

		private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(e.OldValue != e.NewValue && e.NewValue != null)
			{
				DictionaryAssistant.SetResourceDictionary((string)e.NewValue, DictionaryAssistant.Mode.Language);
				ContentControl control = d as ContentControl;
				control.RaiseEvent(new RoutedEventArgs(LanguageChangedEvent, control));
			}
		}

		private static object OnLanguageCoerceValue(DependencyObject d, object value)
		{
			switch((string)value)
			{
				case "Русский":
				{
					return ListOfRootLanguages[0];
				}
				case "Українська":
				{
					return ListOfRootLanguages[1];
				}
				default:
				{
					return ListOfRootLanguages[2];
				}
			}
		}

		public static readonly RoutedEvent LanguageChangedEvent = EventManager.RegisterRoutedEvent("LanguageChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PersonalizationManager));

		public static void AddLanguageChangedHandler(DependencyObject obj, RoutedEventHandler handler) => ((UIElement)obj).AddHandler(LanguageChangedEvent, handler);

		public static void RemoveLanguageChangedHandler(DependencyObject obj, RoutedEventHandler handler) => ((UIElement)obj).RemoveHandler(LanguageChangedEvent, handler);

		#endregion

		#region Theme

		public static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached("Theme", typeof(string), 
			typeof(PersonalizationManager), new PropertyMetadata(string.Empty, OnThemeChanged));

		public static string GetTheme(DependencyObject d) => (string)d.GetValue(ThemeProperty);

		public static void SetTheme(DependencyObject d, string value)
		{

			d.SetValue(ThemeProperty, value);
		}

		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(e.OldValue != e.NewValue && e.NewValue != null)
				DictionaryAssistant.SetResourceDictionary(e.NewValue.ToString(), DictionaryAssistant.Mode.Theme);

		}

		#endregion

		#region Accent

		public static readonly DependencyProperty AccentProperty = DependencyProperty.RegisterAttached("Accent", typeof(string), 
			typeof(PersonalizationManager), new PropertyMetadata(string.Empty, OnAccentChanged));

		public static string GetAccent(DependencyObject d) => (string)d.GetValue(AccentProperty);

		public static void SetAccent(DependencyObject d, string value) => d.SetValue(AccentProperty, value);

		private static void OnAccentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(e.OldValue != e.NewValue && e.NewValue != null)
				DictionaryAssistant.SetResourceDictionary(e.NewValue.ToString(), DictionaryAssistant.Mode.Accent);

		}

		#endregion
	}

}

