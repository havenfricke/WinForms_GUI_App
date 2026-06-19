using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using WinForms_GUI_App.Models;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App
{
    internal class AppState
    {
        // Establish a single global instance of Watcher
        public static Watcher watcher { get; } = new Watcher();

        public class Watcher : DynamicObject
        {
            private readonly Dictionary<string, object> state = new Dictionary<string, object>();
            private static readonly Dictionary<string, object> appMetadata = new Dictionary<string, object>();
            private static App app;

            private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            // Add an event to bridge the dynamic object to WinForms
            // Example usage: AppState.watcher.StateUpdated += OnWatcherStateUpdated;
            public event Action<string, object> StateUpdated;

            // The main constructor initializes the state with default values (See Models/AppMetadata.cs)
            public Watcher()
            {
                state["current_page"] = "Dashboard";
                state["arduino_data"] = "";
                state["selected_port"] = SerialPortListener.listener.GetDefaultComPort();
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                string name = binder.Name;
                state.TryGetValue(name, out object currentValue);

                if (!Equals(currentValue, value))
                {
                    state[name] = value;
                    OnChange(name, value);
                }

                return true;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return state.TryGetValue(binder.Name, out result);
            }

            private void OnChange(string name, object newValue)
            {
                appMetadata[name] = newValue;

                var changeDict = new Dictionary<string, object> { { name, newValue } };
                string changeJson = JsonSerializer.Serialize(changeDict, jsonOptions);

                Debug.WriteLine($"Triggered Update: {changeJson}");

                var metadataSnapshot = new AppMetadata(
                    state["current_page"] as string,
                    state["arduino_data"] as string,
                    state["selected_port"] as string
                );

                // Assuming UserData exists in your environment
                app = new App(UserData.GetAppId(), UserData.GetAppIp(), metadataSnapshot);

                // payload for the POST request
                string payload = JsonSerializer.Serialize(app, jsonOptions);

                Debug.WriteLine($"Updated App Metadata: {payload}");

                // Fire the event to tell WinForms the dynamic state just changed
                StateUpdated?.Invoke(name, newValue);
            }
        }
    }
}