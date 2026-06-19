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
    // Start serial at 9600
    Serial.begin(9600);
    
    // Send a startup message so C# knows the board is alive
    Serial.println("ARDUINO_READY"); 
}

void loop()
{
   // Send outgoing test data
   Serial.println("Sensor data: 42");

   // Check if there are bytes waiting in the serial buffer
   if (Serial.available() > 0)
   {
       // Read strictly until the newline character arrives.
       // This is significantly faster and prevents timeout hangs.
       String incomingMessage = Serial.readStringUntil('\n');
       
       // Remove extra spaces or hidden carriage returns (\r)
       incomingMessage.trim(); 
       
       // Only act if the string isn't blank
       if (incomingMessage.length() > 0) 
       {
           // Echo the message back to the monitor
           Serial.print("Arduino received: ");
           Serial.println(incomingMessage);
       }
   }

   // Wait for 1 second before running the loop again
   delay(1000);
}
```

Sending a command from the C# application to the Arduino will trigger the Arduino to echo back the message, which can be observed in the serial monitor. 
The Arduino code is designed to send a test message every second and listen for incoming messages from the C# application.
