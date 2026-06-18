using System.Diagnostics;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App.UI.Pages
{
    internal class Home : UserControl
    {
        public Home()
        {
            BackColor = Color.Transparent; // Inherits background from the ContentPanel

            // Utilize your existing Header factory
            Header headerFactory = new Header();
            Label title = headerFactory.One("Home Dashboard");
            title.Location = new Point(20, 20);
            StartListening();

            Controls.Add(title);
        }

        // Inside Home.cs
        private void StartListening()
        {
            SerialPortListener.listener.DataReceived += OnArduinoDataReceived;

            // EXPLICITLY CALL LISTEN TO OPEN THE PORT
            SerialPortListener.listener.Listen("COM3", 9600);
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

        private void UpdateUI(string data)
        {
            // Update your specific WinForms controls here
            // For example, you could update a Label or TextBox with the received data
            // labelData.Text = data; // Assuming you have a Label named labelData
            Debug.WriteLine($"Received data: {data}"); // For demonstration, log the received data
        }
    }
}