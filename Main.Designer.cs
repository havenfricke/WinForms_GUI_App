using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WinForms_GUI_App.UI;
using WinForms_GUI_App.UI.Pages;
using WinForms_GUI_App.Utils;
using static WinForms_GUI_App.AppState;

namespace WinForms_GUI_App
{
    partial class Main
    {
        private ContentManager contentManager;
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

            Navigation navComponent = new Navigation();
            navComponent.OnNavigate += NavigationComponent_OnNavigate;

            // 1. Initialize the ContentManager
            contentManager = new ContentManager();

            FlowLayoutPanel navPanel = navComponent.InitializeNavbar();
            Panel contentPanel = contentManager.InitializeContentPanel();

            mainGrid.Controls.Add(navPanel, 0, 0);
            mainGrid.Controls.Add(contentPanel, 1, 0);
            this.Controls.Add(mainGrid);

            // 2. Load the default startup view
            contentManager.LoadView(new Home());
            watcher.current_page = "Home";
            StartListening();
        }

        private void UpdateUI(string data)
        {
            // Update your specific WinForms controls here
            // For example, you could update a Label or TextBox with the received data
            // labelData.Text = data; // Assuming you have a Label named labelData
            Debug.WriteLine($"Received data: {data}"); // For demonstration, log the received data
        }

        // 3. Handle the routing request
        private void NavigationComponent_OnNavigate(object sender, string targetPage)
        {
            System.Diagnostics.Debug.WriteLine($"Loading view: {targetPage}");

            switch (targetPage)
            {
                case "Home":
                    contentManager.LoadView(new Home());
                    watcher.current_page = "Home";
                    StartListening(); 
                    break;
                case "Settings":
                    contentManager.LoadView(new Settings());
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

        private void OnArduinoDataReceived(object sender, string data)
        {
            // Ensure thread safety when updating WinForms controls
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateUI(data)));
            }
            else
            {
                UpdateUI(data);
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