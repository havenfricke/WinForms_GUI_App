using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WinForms_GUI_App.UI;
using WinForms_GUI_App.UI.Components;
using WinForms_GUI_App.UI.Pages;
using WinForms_GUI_App.Utils;
using static WinForms_GUI_App.AppState;

namespace WinForms_GUI_App
{
    partial class Main
    {
        dynamic watcher = new Watcher();

        private async void HandleUserData()
        {
            try
            {
                // Call
                string dataPath = await UserData.InitUserDataAsync();

                // Verify it worked by pulling data from the helper methods
                string appId = UserData.GetAppId();
                Debug.WriteLine($"Initialized successfully!\nPath: {dataPath}\nApp ID: {appId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization failed: {ex.Message}");
            }
        }

        private void InitializeCustomComponents()
        {
            TableLayoutPanel mainGrid = new TableLayoutPanel();
            mainGrid.Dock = DockStyle.Fill;
            mainGrid.ColumnCount = 2;
            mainGrid.RowCount = 1;
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Navigation.nav.OnNavigate += NavigationComponent_OnNavigate;

            FlowLayoutPanel navPanel = Navigation.nav.InitializeNavbar();

            // Reference the singleton's panel instance directly
            Panel contentPanel = ContentPanel.panel.MainPanel;

            mainGrid.Controls.Add(navPanel, 0, 0);
            mainGrid.Controls.Add(contentPanel, 1, 0);
            this.Controls.Add(mainGrid);

            ContentManager.manager.LoadView(new Home());
            watcher.current_page = "Home";
            StartListening();
        }

        // Handle the routing request
        private void NavigationComponent_OnNavigate(object sender, string targetPage)
        {
            System.Diagnostics.Debug.WriteLine($"Loading view: {targetPage}");

            switch (targetPage)
            {
                case "Home":
                    ContentManager.manager.LoadView(new Home());
                    watcher.current_page = "Home";
                    StartListening(); // Only listen for Arduino data on the Home page
                    break;
                case "Settings":
                    ContentManager.manager.LoadView(new Settings());
                    watcher.current_page = "Settings";
                    StopListening();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"View '{targetPage}' not found.");
                    watcher.current_page = "Home";
                    StopListening();
                    break;
            }
        }

        // COM port listening logic
        private void StartListening()
        {
            SerialPortListener.listener.DataReceived += OnArduinoDataReceived;

            // EXPLICITLY CALL LISTEN TO OPEN THE PORT
            SerialPortListener.listener.Listen("COM3", 9600);
        }

        private void StopListening()
        {
            SerialPortListener.listener.Stop();
            SerialPortListener.listener.DataReceived -= OnArduinoDataReceived;
        }

        private void UpdateAppState(string data)
        {
            watcher.arduino_data = data; // Store the latest Arduino data in the watcher
            Debug.WriteLine($"Received data: {watcher.arduino_data}"); // For demonstration, log the received data
        }

        private void OnArduinoDataReceived(object sender, string data)
        {
            // Ensure thread safety when updating WinForms controls
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateAppState(data)));
            }
            else
            {
                UpdateAppState(data);
            }
        }

        #region Windows Form Designer generated code
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(804, 450);
            Name = "App Name";
            Text = "App Name";
            ResumeLayout(false);
        }
        #endregion
    }
}