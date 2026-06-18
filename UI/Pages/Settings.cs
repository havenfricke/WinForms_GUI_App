using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using WinForms_GUI_App.UI.Factories;

namespace WinForms_GUI_App.UI.Pages
{
    internal class Settings : UserControl
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

            Label portLabel = new Label
            {
                Text = "COM Port:",
                ForeColor = Color.White,
                // Assuming UIConstants is available based on your Home page code
                Font = new Font(UIConstants.DefaultFontFamily, UIConstants.ParagraphSize),
                AutoSize = true,
                Anchor = AnchorStyles.Left // Centers the text vertically relative to the dropdown
            };

            portDropdown = new ComboBox();
            portDropdown.Width = 150;
            portDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            portDropdown.Anchor = AnchorStyles.Left; // Aligns it to the left side of its grid cell

            // Populate the dropdown 
            string[] ports = GetAvailablePorts();
            portDropdown.Items.AddRange(ports);

            // Select default or handle empty state
            if (portDropdown.Items.Count > 0)
            {
                portDropdown.SelectedIndex = 0;
            }
            else
            {
                portDropdown.Items.Add("No COM Ports Found");
                portDropdown.SelectedIndex = 0;
                portDropdown.Enabled = false;
            }

            TableLayoutPanel settingsGrid = new TableLayoutPanel();
            settingsGrid.Dock = DockStyle.Fill;
            settingsGrid.Padding = new Padding(20, 0, 0, 0); // Pushes the grid content inward, matching Home
            settingsGrid.ColumnCount = 2;
            settingsGrid.RowCount = 1;
            settingsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            settingsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            settingsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            settingsGrid.Controls.Add(portLabel, 0, 0);
            settingsGrid.Controls.Add(portDropdown, 1, 0);

            Controls.Add(settingsGrid);

            // Note: If you have a title on this page docked to the Top, 
            // ensure you call title.SendToBack() in the constructor after calling this method.
        }

        private string[] GetAvailablePorts()
        {
            try
            {
                return SerialPort.GetPortNames();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching serial ports: {ex.Message}");
                return new string[0];
            }
        }
    }
}