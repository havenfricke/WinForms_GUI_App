### Overview of WinForms_GUI_App

**What it does**

The WinForms_GUI_App is a C# Windows Forms application designed to establish direct, bidirectional serial communication with an Arduino device. 
It provides a graphical user interface (GUI) that allows users to send execution commands to an Arduino and listen for real-time sensor data or 
telemetry broadcasted from the hardware over a COM port.


**How it does it**

The application implements several modern design patterns to work around the traditional limitations of Windows Forms, giving it a Single-Page 
Application (SPA) architecture:


**Serial Communication (SerialPortListener)**

A dedicated singleton handles the hardware interface. It identifies available COM ports, opens the connection, and listens for incoming byte streams. 
When Arduino data is received, it safely marshals the data back to the main UI thread (using InvokeRequired logic) to prevent cross-threading exceptions.


**Global State Management (AppState.Watcher)**

Instead of passing data manually between various UI elements, the app uses a dynamic, globally accessible state object. When the serial listener receives 
new data, it updates the Watcher. UI components (like the Home dashboard) subscribe to these specific property changes (e.g., StateUpdated events) and 
automatically update their local text labels when the hardware pushes new telemetry.


**Dynamic Routing (ContentManager & Navigation)**

The application relies on a single main window (Main.cs) populated by a TableLayoutPanel. A ContentManager singleton acts as a router. When a user clicks 
a navigation button, the ContentManager safely disposes of the currently active view and injects a new UserControl (such as Home.cs or Settings.cs) into 
the primary display panel.


**Programmatic UI Generation**

The UI is largely built in code rather than relying exclusively on the drag-and-drop Windows Forms Designer. It utilizes factory classes 
(like Header and NavButtons) to generate layout structures, apply styling, and wire up event handlers programmatically.

### Arduino Test Code
```cpp
void setup()
{
    // Initialize serial communication at 9600 bits per second
    Serial.begin(9600);
}

void loop()
{
// Send a message followed by a carriage return and newline
    Serial.println("Sensor data: 42");

    // Wait for 1 second
    delay(1000);
}
```
