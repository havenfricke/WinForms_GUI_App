using System;
using System.Diagnostics;
using System.IO.Ports;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App.Utils
{
    internal class SerialPortListener : IDisposable
    {
        public static SerialPortListener listener { get; } = new SerialPortListener();
        private SerialPort serialPort;

        // Event to broadcast incoming data to the WinForms UI
        public event EventHandler<string> DataReceived;

        public void Listen(string portName = "COM3", int baudRate = 9600)
        {
            // Check if the port is already initialized and open
            if (serialPort != null && serialPort.IsOpen)
            {
                Debug.WriteLine("Port is already open and listening.");
                return;
            }

            // Initialize only if not already connected
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

        public void Stop()
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    Debug.WriteLine("Serial port closed.");
                }
                else
                {
                    Debug.WriteLine("Serial port is already closed.");
                }
            }
            else
            {
                Debug.WriteLine("Serial port was not initialized.");
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

// ARDUINO EXAMPLE CODE
//void setup()
//{
//    // Initialize serial communication at 9600 bits per second
//    Serial.begin(9600);
//}

//void loop()
//{
//    // Send a message followed by a carriage return and newline
//    Serial.println("Sensor data: 42");

//    // Wait for 1 second
//    delay(1000);
//}