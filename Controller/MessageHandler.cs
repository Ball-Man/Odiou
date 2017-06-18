using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using System.Windows.Threading;
using System.Windows.Media;

namespace Controller
{
    /// <summary>
    /// Shows messages in a smart way
    /// </summary>
    class MessageHandler
    {
        /// <summary>
        /// Shows messages
        /// </summary>
        private Card _box;

        /// <summary>
        /// Saves the initial box brush
        /// </summary>
        private Brush _boxColor;

        /// <summary>
        /// Let messages disappear
        /// </summary>
        private DispatcherTimer _dispatcher = new DispatcherTimer();

        /// <summary>
        /// Creates an messages handler
        /// </summary>
        /// <param name="box">The card where messages will be shown</param>
        /// 
        public MessageHandler(Card box)
        {
            _box = box;
            _boxColor = _box.Background;
        }

        /// <summary>
        /// Shows the desired error for the desired time
        /// </summary>
        /// <param name="text">The error text</param>
        /// <param name="ms">Time in milliseconds</param>
        public void ShowError(string text, int ms)
        {
            Show(text, ms, new SolidColorBrush(Color.FromRgb(229, 115, 115)));
        }

        /// <summary>
        /// Shows the desired message for the desired time
        /// </summary>
        /// <param name="text">The message text</param>
        /// <param name="ms">Time in milliseconds</param>
        public void ShowMessage(string text, int ms)
        {
            Show(text, ms, new SolidColorBrush(Color.FromRgb(174, 213, 129)));
        }

        /// <summary>
        /// Shows the desired message with the desired color
        /// </summary>
        private void Show(string text, int ms, Brush color)
        {
            _box.Background = color;

            _box.Content = text;

            _dispatcher.Tick += (s, e) =>
            {
                _box.Background = _boxColor;
                _box.Content = "";
                (s as DispatcherTimer).Stop();
            };
            _dispatcher.Interval = new TimeSpan(0, 0, 0, 0, ms);
            _dispatcher.Start();
        }
    }
}
