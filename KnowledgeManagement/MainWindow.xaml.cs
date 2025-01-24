using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.IO;

namespace KnowledgeManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        public MainWindow()
        {
            InitializeComponent();

            // Wait for window to load before positioning
            this.Loaded += (s, e) =>
            {
                PositionWindowAboveMouse();
            };

            // Add key event handler
            QuickNoteTextBox.PreviewKeyDown += QuickNoteTextBox_PreviewKeyDown;
        }

        private void PositionWindowAboveMouse()
        {
            POINT point;
            GetCursorPos(out point);

            // Calculate window position (just above the mouse cursor)
            this.Left = point.X - this.Width / 2;  // Center horizontally on mouse
            this.Top = point.Y - this.Height - 30; // 10 pixels above mouse
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void QuickNoteTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveAndClose();
                e.Handled = true;
            }
        }

        private void SaveAndClose()
        {
            try
            {
                string textToSave = $"\n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n{QuickNoteTextBox.Text}\n";

                var filename = textToSave.Split(Environment.NewLine)[0] + ".txt";

                string desktopPath = "c:/knowledgebase/";
                string filePath = System.IO.Path.Combine(desktopPath, "QuickNotes.txt");

                // Append the text with timestamp                
                File.AppendAllText(filePath, textToSave);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving note: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}