using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SkyTrekVisual.Controls
{

	[TemplateVisualState(Name = "ShowGameOverLayout", GroupName = "ViewGameOverStates"),
	TemplateVisualState(Name = "HideGameOverLayout", GroupName = "ViewGameOverStates"),
	TemplateVisualState(Name = "ShowPauseLayout", GroupName = "ViewPauseStates"),
	TemplateVisualState(Name = "HidePauseLayout", GroupName = "ViewPauseStates"),
    TemplatePart(Name = "PauseLayoutHidingStoryboard", Type = typeof(Storyboard)),
    TemplatePart(Name = "GameOverLayoutHidingStoryboard", Type = typeof(Storyboard))]
	public class LayoutManager : Control
	{

		UIElement gameplayLayout = null;
        UIElement pauseLayout = null;
        UIElement gameOverLayout = null;

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
		/// GAMEOVER
		/// </summary>
		public static readonly DependencyProperty GameOverLayoutProperty = DependencyProperty.Register("GameOverLayout", typeof(object), typeof(LayoutManager), null);

		public object GameOverLayout
        {
			get { return GetValue(GameOverLayoutProperty); }
			set { SetValue(GameOverLayoutProperty, value); }
		}


		#region Pause

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
			(d as LayoutManager).DisplayPause((bool)e.NewValue);
		}

		#endregion

		#region GameOver
		/// <summary>
		/// IS GAMEOVER
		/// </summary>
		public static readonly DependencyProperty IsGameOverProperty = DependencyProperty.Register("IsGameOver", typeof(bool), typeof(LayoutManager), new PropertyMetadata(false, OnGameOver));

        private static void OnGameOver(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LayoutManager).DisplayGameOver((bool)e.NewValue);
        }

        public bool IsGameOver
        {
            get { return (bool)GetValue(IsGameOverProperty); }
            set { SetValue(IsGameOverProperty, value); }
        }

		#endregion



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

            Storyboard gameOverLayoutHidingStoryboard = base.GetTemplateChild("GameOverLayoutHidingStoryboard") as Storyboard;
            if (gameOverLayoutHidingStoryboard != null)
                gameOverLayoutHidingStoryboard.Completed += GameOverLayoutHidingStoryboard_Completed;

            gameplayLayout = GameplayLayout as UIElement;
            pauseLayout = PauseLayout as UIElement;
			gameOverLayout = GameOverLayout as UIElement;


            pauseLayout.Visibility = Visibility.Hidden;
            gameOverLayout.Visibility = Visibility.Hidden;
		}

        private void GameOverLayoutHidingStoryboard_Completed(object sender, EventArgs e)
        {
            if (gameOverLayout != null)
            {
                gameOverLayout.Visibility = Visibility.Hidden;
            }
        }

        private void PauseLayoutHidingStoryboard_Completed(object sender, EventArgs e)
		{
			if(pauseLayout != null)
			{
				pauseLayout.Visibility = Visibility.Hidden;
			}
		}

		private void DisplayPause(bool isPause)
		{
            if (isPause)
            {
                VisualStateManager.GoToState(this, "ShowPauseLayout", true);
                pauseLayout.Visibility = Visibility.Visible;
            }
            else
                VisualStateManager.GoToState(this, "HidePauseLayout", true);
				
		}

        private void DisplayGameOver(bool isGameOver)
        {
			if(isGameOver)
			{
                (GameOverLayout as UIElement).Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, "ShowGameOverLayout", true);
			}
			else
                VisualStateManager.GoToState(this, "HideGameOverLayout", true);
        }


	}
}
