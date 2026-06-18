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
        private Label dataLabel;

        public Home()
        {
            BackColor = Color.Transparent;

            // Setup Header
            Header headerFactory = new Header();
            Label title = headerFactory.One("Home Dashboard");
            title.Location = new Point(20, 20);

            Controls.Add(title);
            title.SendToBack();

            // Initialize the main data grid layout
            InitializeDataGrid();

            // Subscribe to the concrete event to listen for dynamic updates
            AppState.watcher.StateUpdated += OnWatcherStateUpdated;
        }

        private void InitializeDataGrid()
        {
            dynamic watcher = AppState.watcher;

            TableLayoutPanel dataGrid = new TableLayoutPanel();
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.ColumnCount = 2;
            dataGrid.RowCount = 1;
            dataGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            dataGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            dataGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            dataLabel = new Label
            {
                Text = "Arduino Data:",
                ForeColor = Color.White,
                Font = new Font(UIConstants.DefaultFontFamily, UIConstants.ParagraphSize),
                AutoSize = true,
                Anchor = AnchorStyles.Left // Centers the text vertically in the grid cell
            };

            arduinoData = new Label
            {
                Text = watcher.arduino_data,
                ForeColor = Color.White,
                Font = new Font(UIConstants.DefaultFontFamily, UIConstants.ParagraphSize),
                AutoSize = true,
                Anchor = AnchorStyles.Left // Centers the text vertically in the grid cell
            };

            dataGrid.Controls.Add(dataLabel, 0, 0);
            dataGrid.Controls.Add(arduinoData, 1, 0);

            // Add Grid to the UserControl
            Controls.Add(dataGrid);
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