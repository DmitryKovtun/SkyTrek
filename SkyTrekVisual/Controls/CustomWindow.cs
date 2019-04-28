using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SkyTrekVisual.Controls
{
	public class CustomWindow : Window
	{




		public static readonly DependencyProperty ShowScreenSaverProperty = DependencyProperty.Register("ShowScreenSaver", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false, OnShowScreenSaverPropertyChangedCallback));

		public bool ShowScreenSaver
		{
			get { return (bool)GetValue(ShowScreenSaverProperty); }
			set { SetValue(ShowScreenSaverProperty, value); }
		}

		static void SetShowScreenSaver(DependencyObject d, bool value) => d.SetValue(ShowScreenSaverProperty, value);

		private static void OnShowScreenSaverPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SetShowScreenSaver(d, (bool)e.NewValue);
		}

		public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true));

		public bool ShowMinButton
		{
			get { return (bool)GetValue(ShowMinButtonProperty); }
			set { SetValue(ShowMinButtonProperty, value); }
		}

		public static readonly DependencyProperty ShowSettingsProperty = DependencyProperty.Register("ShowSettings", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false));

		public bool ShowSettings
		{
			get { return (bool)GetValue(ShowSettingsProperty); }
			set { SetValue(ShowSettingsProperty, value); }
		}

		public static readonly DependencyProperty ShowBackProperty = DependencyProperty.Register("ShowBack", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false));

		public bool ShowBack
		{
			get { return (bool)GetValue(ShowBackProperty); }
			set { SetValue(ShowBackProperty, value); }
		}

		public static readonly DependencyProperty ShowSearchProperty = DependencyProperty.Register("ShowSearch", typeof(bool), typeof(CustomWindow), new PropertyMetadata(false));

		public bool ShowSearch
		{
			get { return (bool)GetValue(ShowSearchProperty); }
			set { SetValue(ShowSearchProperty, value); }
		}









		public CustomWindow()
		{
			CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
			CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));
			Style = (Style)TryFindResource("CustomWindow");

		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			//(GetTemplateChild("SettingsButton") as Button).Click += delegate { RaiseEvent(new RoutedEventArgs(SettingsButtonClickEvent)); };
		}

		private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;

		private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = ResizeMode != ResizeMode.NoResize;

		private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(this);

		private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.MaximizeWindow(this);

		private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.MinimizeWindow(this);

		private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.RestoreWindow(this);

		public static readonly RoutedEvent SettingsButtonClickEvent = EventManager.RegisterRoutedEvent("SettingsButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomWindow));

		public event RoutedEventHandler SettingsButtonClick
		{
			add { AddHandler(SettingsButtonClickEvent, value); }
			remove { RemoveHandler(SettingsButtonClickEvent, value); }
		}

	}

}
