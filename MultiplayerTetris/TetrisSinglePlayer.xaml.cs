using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MultiplayerTetris
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TetrisSinglePlayer : MultiplayerTetris.Common.LayoutAwarePage
    {
        int level = 0;
        DispatcherTimer timer;
        Tetris.SPGameController gc;
        int state = 0; //0 = pageLoad... 1= paused... 2 = playing...3 = ended

        public TetrisSinglePlayer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            /*
            if (state == 0)
            {
                state = 2;
                timer.Start();
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ((Button)this.FindName("restartButton")).Content = "Start";
            }
             * */
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (state == 0)
            {
                gc = new Tetris.SPGameController(this,level);
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ((Button)this.FindName("restartButton")).Content = "Start";
            }
            ((TextBlock)this.FindName("levelText")).Text = "Level  : " + ((Slider)this.FindName("levelSlider")).Value.ToString();
            ((Button)this.FindName("drop")).Focus(Windows.UI.Xaml.FocusState.Programmatic);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(70);
            timer.Tick += timer_Tick;
            Window.Current.Content.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(keyDownHandler), true);
        }
        public void gameOver()
        {
            this.state = 3;
            ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ((Button)this.FindName("restartButton")).Content = "Start";
        }

        void keyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if(state==2)
                gc.key(e.Key);
            else if (state == 0)
            {
                state = 2;
                timer.Start();
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Visible;
                ((Button)this.FindName("restartButton")).Content = "Restart";
            }
        }
        void timer_Tick(object sender, object e)
        {
            if(state==2)
                gc.tick();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Button)this.FindName("drop")).Focus(Windows.UI.Xaml.FocusState.Programmatic);
            if (state == 3)
            {
                ((Button)this.FindName("pauseButton")).Content = "Pause";
                state = 2;
            } else if (state == 2)
            {
                ((Button)this.FindName("pauseButton")).Content = "Resume";
                state = 3;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((Button)this.FindName("drop")).Focus(Windows.UI.Xaml.FocusState.Programmatic);
            if(state == 0)
            {
                state = 2;
                timer.Start();
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Visible;
                ((Button)this.FindName("restartButton")).Content = "Restart";
            }
            else if (state == 2)
            {
                gc = new Tetris.SPGameController(this, level);
                state = 0;
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ((Button)this.FindName("restartButton")).Content = "Start";
            }
            else if (state == 3)
            {
                gc = new Tetris.SPGameController(this, level);
                state = 2;
                timer.Start();
                ((Button)this.FindName("pauseButton")).Visibility = Windows.UI.Xaml.Visibility.Visible;
                ((Button)this.FindName("restartButton")).Content = "Restart";
            }
        }

        private void drop_Click(object sender, RoutedEventArgs e)
        {
            if (state == 2)
                gc.key(Windows.System.VirtualKey.Down);
        }

        private void levelSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            TextBlock tb = ((TextBlock)this.FindName("levelText"));
            if (tb!= null)
            {
                level = (int)e.NewValue-1;
                tb.Text = "Level  : " + (level+1);
                gc.changeLevel(level);
                ((Button)this.FindName("drop")).Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
        }

    }
}
