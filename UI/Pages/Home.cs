using System;
using System.Drawing;
using System.Windows.Forms;
using WinForms_GUI_App.UI.Factories;
// Ensure your Utils and constants namespaces are included

namespace WinForms_GUI_App.UI.Pages
{
    internal class Home : UserControl
    {
        private Label arduinoData;

        public Home()
        {
            BackColor = Color.Transparent;

            Header headerFactory = new Header();
            Label title = headerFactory.One("Home Dashboard");
            title.Location = new Point(20, 20);

            // 1. Cast the global instance to dynamic to read its initial properties
            dynamic watcher = AppState.watcher;

            arduinoData = new Label
            {
                Text = watcher.arduino_data, // Grabs the initial string
                ForeColor = Color.White,
                Font = new Font(UIConstants.DefaultFontFamily, UIConstants.ParagraphSize),
                AutoSize = true,
                Location = new Point(20, 90)
            };

            Controls.Add(title);
            Controls.Add(arduinoData);

            // Subscribe to the concrete event to listen for dynamic updates
            AppState.watcher.StateUpdated += OnWatcherStateUpdated;
        }

        // Update the UI safely when the Watcher changes
        private void OnWatcherStateUpdated(string key, object newValue)
        {
            // Only update the label if the arduino_data property is what changed
            if (key == "arduino_data")
            {
                string updatedText = newValue?.ToString() ?? string.Empty;

                if (arduinoData.InvokeRequired)
                {
                    arduinoData.Invoke(new Action(() => arduinoData.Text = updatedText));
                }
                else
                {
                    arduinoData.Text = updatedText;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Prevent memory leaks
                AppState.watcher.StateUpdated -= OnWatcherStateUpdated;
            }
            base.Dispose(disposing);
        }
    }
}