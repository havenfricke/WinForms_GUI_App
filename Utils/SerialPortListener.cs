using System.Diagnostics;
using System.IO.Ports;

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
            serialPort.DtrEnable = true;
            serialPort.ReadTimeout = 2000; // Drop the read attempt if \n is not received within 500ms
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
                string incomingData = sp.ReadLine().Trim();

                // Pass the data directly into the global app state
                dynamic state = AppState.watcher;
                state.arduino_data = incomingData;
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

        public void SendCommand(string command)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    // WriteLine automatically appends a newline character, 
                    // which helps the Arduino know when the message is complete.
                    serialPort.WriteLine(command);
                    Debug.WriteLine($"Sent: {command}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to send data: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("Cannot send message. Port is closed or not initialized.");
            }
        }

        public string GetDefaultComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length > 0)
                {
                    return ports[0]; // Return the first available port
                }
            }
            catch
            {
                // Suppress or log hardware polling errors
            }

            return "None";
        }

        public string[] GetAvailablePorts()
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