using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Controller
{
    /// <summary>
    /// Manages grids and menu
    /// </summary>
    class MenuController
    {
        /// <summary>
        /// Buttons used to change grids
        /// </summary>
        private Button[] _menu;

        /// <summary>
        /// Grids representing the different 'views' of the app
        /// </summary>
        private Grid[] _tabs;

        /// <summary>
        /// Creates a menu controller for the given grids
        /// </summary>
        /// <param name="menu">Vector of buttons representing the menu</param>
        /// <param name="tabs">Vector of grids representing the app different 'views'</param>
        public MenuController(Button[] menu, Grid[] tabs)
        {
            _menu = menu;
            _tabs = tabs;
        }

        /// <summary>
        /// Switches to corresponding menu tab
        /// </summary>
        /// <param name="sender">The pressed button</param>
        public void SwitchTab(Button sender)
        {
            Collapse();
            _tabs[Array.IndexOf(_menu, sender)].Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Enables or disables every tab
        /// </summary>
        /// <param name="enabled">true: enables, false: disables</param>
        public void Enable(bool enabled)
        {
            foreach (Grid g in _tabs)
                g.IsEnabled = enabled;
        }

        /// <summary>
        /// Enables or disables every tab but a specific one
        /// </summary>
        /// <param name="sender">The selected grid</param>
        /// <param name="enabled">true: enables, false: disables</param>
        public void EnableBut(Grid sender, bool enabled)
        {
            Enable(enabled);
            sender.IsEnabled = !enabled;
        }

        /// <summary>
        /// Collapses every grid
        /// </summary>
        private void Collapse()
        {
            foreach (Grid g in _tabs)
                g.Visibility = Visibility.Collapsed;
        }
    }
}
