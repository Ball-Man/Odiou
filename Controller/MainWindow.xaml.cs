using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Microsoft.Win32;
using Odiou;
using WindowsInput;

namespace Controller
{
    /// <summary>
    /// Logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ************* Constants and fields ************* //
        // *************                      ************* //
        const int DEFAULTDEVICE = 0,
            DEFAULTFREQUENCY = 48000,
            DEFAULTDEPTH = 32,
            DEFAULTCHANNELS = 2,
            DEFAULTBUFFER = 100;

        //Controller for the live audio recognition
        LiveAudioController liveController;
        NoteInterpreter interpreter;

        //Controller for the menu tab switching
        MenuController menu;

        //Shows error messages
        MessageHandler message;

        // ************* Window ************* //
        // *************        ************* //
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Notes initialization
            FFT.ChargeNotes("notes.config");

            //Message handler initialization
            message = new MessageHandler(crdError);

            //Live controller initialization
            liveController = new LiveAudioController(new AudioRecorder(1, 48000, 32, 2, 100), txtNote, this.Dispatcher);

            //Menu initialization
            Button[] menuButtons = new Button[] { btnNote, btnAnalysis, btnSettings };
            Grid[] menuGrids = new Grid[] { grdPlay, grdAnalysis, grdSettings };
            menu = new MenuController(menuButtons, menuGrids);

            //Settings initialization
            cmbDevice.ItemsSource = AudioRecorder.Devices;
            cmbDevice.SelectedIndex = 0;

            interpreter = new NoteInterpreter();
            liveController.Interpreter = interpreter;
            Bind();
        }

        /// <summary>
        /// Binds and refreshes listbox
        /// </summary>
        private void Bind()
        {
            lstCommands.ItemsSource = liveController.Interpreter.Associations;
            lstCommands.Items.Refresh();
        }

        // ************* Menu ************* //
        // *************      ************* //
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Button snd = sender as Button;
            menu.SwitchTab(snd);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseClick(object sender, MouseButtonEventArgs e)
        {
            //Window drag and drop
            this.DragMove();
        }

        // ************* Live ************* //
        // *************      ************* //
        private void btnStartLive_Click(object sender, RoutedEventArgs e)
        {
            btnStartLive.IsEnabled = false;
            btnStopLive.IsEnabled = true;
            lstCommands.IsEnabled = false;

            try
            {
                //Will throw an exception if an audio device is used with the wrong settins
                liveController.Start();
            }
            catch(Exception)
            {
                message.ShowError("Unsupported wave format, revise your settings", 1500);
                btnStartLive.IsEnabled = true;
                btnStopLive.IsEnabled = false;
            }
        }

        private void btnStopLive_Click(object sender, RoutedEventArgs e)
        {
            StopLive();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            liveController.Interpreter.Associations.Remove(button.DataContext as NoteControl);
            lstCommands.Items.Refresh();
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Note note = Note.Parse(txtNoteInput.Text);
                VirtualKeyCode key = (VirtualKeyCode)cmbKeyInput.SelectedItem;
                bool keep = (bool)chcKeepInput.IsChecked;
                int ms = int.Parse(txtDelayInput.Text);

                SetDefaultInputCommand();
                liveController.Interpreter.Associations.Add(new NoteControl(note, key, keep, ms));
                lstCommands.Items.Refresh();
            }
            catch(Exception)
            {
                message.ShowError("Wrong data", 1000);
            }
        }

        private void btnSaveCommands_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "file xml(.xml)|*.xml|tutti i file|*.*";
            dialog.DefaultExt = ".xml";
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    liveController.Interpreter.Save(dialog.FileName);
                }
                catch(Exception)
                {
                    message.ShowError("Can't open the given file", 1000);
                }
            }
        }

        private void btnLoadCommands_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "file xml(.xml)|*.xml|tutti i file|*.*";
            dialog.DefaultExt = ".xml";
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    liveController.Interpreter.Load(dialog.FileName);
                    Bind();
                }
                catch (Exception)
                {
                    message.ShowError("Can't open the given file", 1000);
                }
            }
        }

        /// <summary>
        /// Stops the live execution
        /// </summary>
        private void StopLive()
        {
            btnStartLive.IsEnabled = true;
            btnStopLive.IsEnabled = false;
            lstCommands.IsEnabled = true;

            liveController.Stop();
        }

        /// <summary>
        /// Sets input form to its default values
        /// </summary>
        private void SetDefaultInputCommand()
        {
            txtNoteInput.Text = "A0";
            cmbKeyInput.SelectedIndex = 0;
            chcKeepInput.IsChecked = false;
            txtDelayInput.Text = "0";
        }

        // ************* Settings ************* //
        // *************          ************* //
        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            int deviceID, freq, depth, channels, buffer;

            //Parses all the settings
            deviceID = cmbDevice.SelectedIndex;

            var tmp = SettingFieldParse(txtSampleRate.Text, DEFAULTFREQUENCY);
            freq = tmp.Item1;

            tmp = SettingFieldParse(txtBitDepth.Text, DEFAULTDEPTH);
            depth = tmp.Item1;

            tmp = SettingFieldParse(txtChannels.Text, DEFAULTCHANNELS);
            channels = tmp.Item1;

            tmp = SettingFieldParse(txtBuffer.Text, DEFAULTBUFFER);
            buffer = tmp.Item1;

            //Stops live execution and sets the new audio device
            StopLive();
            liveController = new LiveAudioController(new AudioRecorder(deviceID, freq, depth, channels, buffer), txtNote, this.Dispatcher);
            liveController.Interpreter = interpreter;
            Bind();

            //Enables everything(everything is disabled till the user chooses the settings)
            menu.Enable(true);

            //Confirmation message
            message.ShowMessage("Settings saved", 1000);
        }

        /// <summary>
        /// Parses and integer from a string
        /// </summary>
        /// <param name="field">The string to be parsed</param>
        /// <param name="def">The default value to be returned</param>
        /// <returns>a tuple(value, bool) where value is the parsed value(or the default one) and bool says if field has been parsed correctly</returns>
        private Tuple<int, bool> SettingFieldParse(string field, int def)
        {
            int n;
            bool ok = int.TryParse(field, out n);

            return new Tuple<int, bool>(ok ? n : def, ok);
        }

        // OTHER
        private void Press()
        {
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_A);
        }
    }
}
