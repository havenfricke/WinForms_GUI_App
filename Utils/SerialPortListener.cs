using System;
using System.Diagnostics;
using System.IO.Ports;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App.Utils
{
    internal class SerialPortListener : IDisposable
    {
        private SerialPort serialPort;

        // Event to broadcast incoming data to the WinForms UI
        public event EventHandler<string> DataReceived;

        public void Listen(string portName = "COM3", int baudRate = 9600)
        {
            // Do not use a 'using' block here; the object must remain alive in memory.
            serialPort = new SerialPort(portName, baudRate);
            serialPort.DataReceived += DataReceivedHandler;

            try
            {
                serialPort.Open();
                Debug.WriteLine($"Connected to {portName}.");
            }
            catch (UnauthorizedAccessException)
            {
                Debug.WriteLine("Error: Port is already in use by another program.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening port: {ex.Message}");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            try
            {
                string incomingData = sp.ReadLine();

                // Pass the data out of this utility class via the event
                DataReceived?.Invoke(this, incomingData.Trim());
            }
            catch (TimeoutException)
            {
                Debug.WriteLine("Read operation timed out.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Serial read error: {ex.Message}");
            }
        }

        // Required to cleanly release the COM port when the application closes
        public void Dispose()
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                serialPort.Dispose();
            }
        }
    }
}


//// Example implementation
//private SerialPortListener _listener;

//private void StartListening()
//{
//    listener = new SerialPortListener();
//    listener.DataReceived += OnArduinoDataReceived;
//    listener.Listen("COM3", 9600);
//}

//private void OnArduinoDataReceived(object sender, string data)
//{
//    // Ensure thread safety when updating WinForms controls
//    if (this.InvokeRequired)
//    {
//        this.Invoke(new Action(() => UpdateUI(data)));
//    }
//    else
//    {
//        UpdateUI(data);
//    }
//}

//private void UpdateUI(string data)
//{
//    // Update your specific WinForms controls here
//    myTextBox.AppendText(data + Environment.NewLine);
//}