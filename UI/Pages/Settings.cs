using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using WinForms_GUI_App.UI.Factories;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App.UI.Pages
{
    internal class Settings : UserControl // reusable, composite user interface 
    {
        // 1. Declare a ComboBox variable at the class level
        private ComboBox portDropdown;

        public Settings()
        {
            BackColor = Color.Transparent;

            Header headerFactory = new Header();
            Label title = headerFactory.One("Settings");
            title.Location = new Point(20, 20);

            Controls.Add(title);
            title.SendToBack();

            InitializePortDropdown();
        }

        private void InitializePortDropdown()
        {
            dynamic watcher = AppState.watcher;

            TableLayoutPanel settingsGrid = new TableLayoutPanel();
            settingsGrid.Dock = DockStyle.Fill;
            settingsGrid.Padding = new Padding(20, 0, 0, 0);
            settingsGrid.ColumnCount = 2;
            settingsGrid.RowCount = 1;
            settingsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            settingsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            settingsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label portLabel = new Label
            {
                Text = "Available COM Ports:",
                ForeColor = Color.White,
                Font = new Font(UIConstants.DefaultFontFamily, UIConstants.ParagraphSize),
                AutoSize = true,
                Anchor = AnchorStyles.Left
            };

            portDropdown = new ComboBox();
            portDropdown.Width = 150;
            portDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            portDropdown.Anchor = AnchorStyles.Left;

            // Populate the dropdown 
            string[] ports = SerialPortListener.listener.GetAvailablePorts();

            if (ports.Length > 0)
            {
                portDropdown.Items.AddRange(ports);

                // Read the globally selected port from AppState, defaulting to index 0 if not found
                string activePort = watcher.selected_port;
                int index = portDropdown.Items.IndexOf(activePort);

                // If the port exists in the list, select it. Otherwise, default to the first item.
                portDropdown.SelectedIndex = index >= 0 ? index : 0;
            }
            else
            {
                portDropdown.Items.Add("No COM Ports Found");
                portDropdown.SelectedIndex = 0;
                portDropdown.Enabled = false;
            }

            // Subscribe to the event AFTER setting the initial index. 
            // This prevents the event from misfiring during UI construction.
            portDropdown.SelectedIndexChanged += (sender, e) =>
            {
                string selectedPort = portDropdown.SelectedItem.ToString();
                Debug.WriteLine($"Selected COM Port: {selectedPort}");

                // Push user changes back to the global state
                watcher.selected_port = selectedPort;
            };

            settingsGrid.Controls.Add(portLabel, 0, 0);
            settingsGrid.Controls.Add(portDropdown, 1, 0);

            Controls.Add(settingsGrid);
        }
    }
}