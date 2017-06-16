using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using Odiou;
using WindowsInput;

namespace Controller
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LiveAudioController controller;

        public MainWindow()
        {
            InitializeComponent();
            FFT.ChargeNotes("notes.config");
            controller = new LiveAudioController(new AudioRecorder(1, 48000, 32, 2, 100), btnStart, btnStop, txtNote, this.Dispatcher);
        }

        private void Press()
        {
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A);
        }

        private void Window_MouseClick(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
