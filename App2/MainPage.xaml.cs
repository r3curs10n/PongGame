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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initialise();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Canvas.SetLeft(player1bat, 50);
            
        }

         Storyboard _gameLoop = new Storyboard();


         void keydown(Windows.UI.Core.CoreWindow win, Windows.UI.Core.KeyEventArgs e)
         {
             if (e.VirtualKey == Windows.System.VirtualKey.S)
             {
                 downPressed = true;
             }
             if (e.VirtualKey == Windows.System.VirtualKey.W)
             {
                 upPressed = true;
             }


         }

         void keyup(Windows.UI.Core.CoreWindow win, Windows.UI.Core.KeyEventArgs e)
         {
             if (e.VirtualKey == Windows.System.VirtualKey.S)
             {
                 downPressed = false;
             }
             if (e.VirtualKey == Windows.System.VirtualKey.W)
             {
                 upPressed = false;
             }

             if (e.VirtualKey == Windows.System.VirtualKey.Space)
             {
                 if (!paused)
                 {
                     txtPaused.Visibility = Windows.UI.Xaml.Visibility.Visible;
                 }
                 else
                 {
                     txtPaused.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                 }
                 paused = !paused;
                 
             }

         }

        private void Initialise()
        {
            _gameLoop.Duration = TimeSpan.FromMilliseconds(0);

            //Once the duration has completed run the following method
            _gameLoop.Completed += GameLoop;

            Window.Current.CoreWindow.KeyDown += new TypedEventHandler<Windows.UI.Core.CoreWindow, Windows.UI.Core.KeyEventArgs>(keydown);
            Window.Current.CoreWindow.KeyUp += new TypedEventHandler<Windows.UI.Core.CoreWindow, Windows.UI.Core.KeyEventArgs>(keyup);
            //Begin executing the storyboard

            _mx = (int) canvas.Width;
            _my = (int) canvas.Height;

            _gameLoop.Begin();
        }

        void GameLogic()
        {
            
            if (downPressed)
                Canvas.SetTop(player1bat, Canvas.GetTop(player1bat) + 5);

            if (upPressed)
                Canvas.SetTop(player1bat, Canvas.GetTop(player1bat) - 5);

            Canvas.SetLeft(ball, Canvas.GetLeft(ball) + vx);
            Canvas.SetTop(ball, Canvas.GetTop(ball) + vy);

            if (Canvas.GetLeft(ball) > _mx || Canvas.GetLeft(ball) < 0) { vx *= -1; s2++; score2.Text = s2.ToString(); }
            if (Canvas.GetTop(ball) > _my || Canvas.GetTop(ball) < 0) vy *= -1;

            Canvas.SetTop(player2bat, Canvas.GetTop(ball) - player2bat.Height / 2);
            
            if ( Math.Abs(Canvas.GetLeft(ball) - (Canvas.GetLeft(player1bat) + player1bat.Width)) <= 5 &&
                Canvas.GetTop(ball) < Canvas.GetTop(player1bat) + player1bat.Height && Canvas.GetTop(ball)+ball.Height > Canvas.GetTop(player1bat)
                )
            {
                vx *= -1;
                tm++;
            }

            if (Math.Abs(Canvas.GetLeft(ball) + ball.Width - (Canvas.GetLeft(player2bat))) <= 5 &&
                Canvas.GetTop(ball) < Canvas.GetTop(player2bat) + player2bat.Height && Canvas.GetTop(ball) > Canvas.GetTop(player2bat)
                )
            {
                vx *= -1;
            }

            int x = tm / 5;
            score1.Text = x.ToString();
            vx /= Math.Abs(vx);
            vy /= Math.Abs(vy);
            vx = (5 + x) * vx;
            vy = (5 + x) * vy;

        }


        private void GameLoop(object sender, object e)
        {
            TimeSpan delayTime = new TimeSpan(0, 0, 0, 0, 0);
            //Take the start time
            TimeSpan beginTime = new TimeSpan(
     DateTime.Now.Day,
     DateTime.Now.Hour,
     DateTime.Now.Minute,
     DateTime.Now.Second,
     DateTime.Now.Millisecond);

            //Execute all game logic

            if (!paused)
                GameLogic();

            //set the delay to zero
            TimeSpan msSpan = new TimeSpan(0);

            //take the end time
            TimeSpan endTime = new TimeSpan(
    DateTime.Now.Day,
    DateTime.Now.Hour,
    DateTime.Now.Minute,
    DateTime.Now.Second,
    DateTime.Now.Millisecond);

            //if the end time is less than the delay time + begin time add some delay, otherwise don’t add delay
            if (endTime < beginTime + delayTime)
            {
                msSpan = ((beginTime + delayTime) - endTime);
            }

            //change the delay before the beginning of the next execution
            _gameLoop.Duration = TimeSpan.FromMilliseconds(msSpan.TotalMilliseconds);
            //restart our gameloop
            _gameLoop.Begin();
        }

        bool upPressed = false;
        bool downPressed = false;

        int vx = -5, vy = 5; int _mx = 991, _my=527;
        int s1 = 0; int s2 = 0; int tm = 0;

        bool paused = true;

    }
}
