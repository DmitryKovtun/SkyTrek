using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SkyTrekVisual.Controls
{

	[TemplateVisualState(Name = "MenuMode", GroupName = "ViewStates"),
	TemplateVisualState(Name = "GameplayMode", GroupName = "ViewStates"),
	TemplateVisualState(Name = "ShowPauseLayout", GroupName = "ViewPauseStates"),
	TemplateVisualState(Name = "HidePauseLayout", GroupName = "ViewPauseStates"),
	TemplatePart(Name = "PauseLayoutHidingStoryboard", Type = typeof(Storyboard))]
	public class LayoutManager : Control
	{
		UIElement pauseLayout = null;
		UIElement gameplayLayout = null;
		UIElement menuLayout = null;

		/// <summary>
		/// GAMEPLAY
		/// </summary>
		public static readonly DependencyProperty GameplayLayoutProperty = DependencyProperty.Register("GameplayLayout", typeof(object), typeof(LayoutManager), null);

		public object GameplayLayout
		{
			get { return GetValue(GameplayLayoutProperty); }
			set { SetValue(GameplayLayoutProperty, value); }
		}

		/// <summary>
		/// PAUSE
		/// </summary>
		public static readonly DependencyProperty PauseLayoutProperty = DependencyProperty.Register("PauseLayout", typeof(object), typeof(LayoutManager), null);

		public object PauseLayout
		{
			get { return GetValue(PauseLayoutProperty); }
			set { SetValue(PauseLayoutProperty, value); }
		}

		/// <summary>
		/// MENU
		/// </summary>
		public static readonly DependencyProperty MenuLayoutProperty = DependencyProperty.Register("MenuLayout", typeof(object), typeof(LayoutManager), null);

		public object MenuLayout
		{
			get { return GetValue(MenuLayoutProperty); }
			set { SetValue(MenuLayoutProperty, value); }
		}

		/// <summary>
		/// IS PAUSE
		/// </summary>
		public static readonly DependencyProperty IsPauseProperty = DependencyProperty.Register("IsPause", typeof(bool), typeof(LayoutManager), new PropertyMetadata(false, OnIsPause));

		public bool IsPause
		{
			get { return (bool)GetValue(IsPauseProperty); }
			set { SetValue(IsPauseProperty, value); }
		}


		private static void OnIsPause(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as LayoutManager).ShowPauseLayout((bool)e.NewValue);
		}

		/// <summary>
		/// IS GAMEPLAY
		/// </summary>
		public static readonly DependencyProperty IsGameplayProperty = DependencyProperty.Register("IsGameplay", typeof(bool), typeof(LayoutManager), new PropertyMetadata(false, OnIsGameplay));

		public bool IsGameplay
		{
			get { return (bool)GetValue(IsGameplayProperty); }
			set { SetValue(IsGameplayProperty, value); }
		}


		private static void OnIsGameplay(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as LayoutManager).ChangeDisplayMode((bool)e.NewValue);
		}

		public LayoutManager()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutManager), new FrameworkPropertyMetadata(typeof(LayoutManager)));
		}


		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Storyboard pauseLayoutHidingStoryboard = base.GetTemplateChild("PauseLayoutHidingStoryboard") as Storyboard;
			if(pauseLayoutHidingStoryboard != null)
				pauseLayoutHidingStoryboard.Completed += PauseLayoutHidingStoryboard_Completed;


			pauseLayout = PauseLayout as UIElement;
			gameplayLayout = GameplayLayout as UIElement;
			menuLayout = MenuLayout as UIElement;

			pauseLayout.Visibility = Visibility.Hidden;

			ChangeDisplayMode(false);

			ShowPauseLayout(false);
		}

		private void PauseLayoutHidingStoryboard_Completed(object sender, EventArgs e)
		{
			if(pauseLayout != null)
			{
				pauseLayout.Visibility = Visibility.Hidden;
			}
		}

		private void ShowPauseLayout(bool isPause)
		{
			if(isPause)
				VisualStateManager.GoToState(this, "ShowPauseLayout", isPause);
			else
				VisualStateManager.GoToState(this, "HidePauseLayout", isPause);

			if(pauseLayout != null && isPause)
				pauseLayout.Visibility = Visibility.Visible;
		}


		private void ChangeDisplayMode(bool isGameplay)
		{
			if(isGameplay)
			{
				VisualStateManager.GoToState(this, "GameplayLayout", isGameplay);
			}
			else
			{
				VisualStateManager.GoToState(this, "MenuLayout", isGameplay);
			}

			if(gameplayLayout != null)
			{
				if(isGameplay)
					gameplayLayout.Visibility = Visibility.Visible;
				else
					gameplayLayout.Visibility = Visibility.Hidden;
			}

			if(menuLayout != null)
			{
				if(isGameplay)
					menuLayout.Visibility = Visibility.Hidden;
				else
					menuLayout.Visibility = Visibility.Visible;
			}

		}

	}
}
