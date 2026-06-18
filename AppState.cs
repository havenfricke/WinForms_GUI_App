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
        // 1. Establish a single global instance of your Watcher
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

            // 2. Add an event to bridge the dynamic object to WinForms
            public event Action<string, object> StateUpdated;

            public Watcher()
            {
                state["current_page"] = "Dashboard";
                state["arduino_data"] = "";
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
                    state["arduino_data"] as string
                );

                // Assuming UserData exists in your environment
                app = new App(UserData.GetAppId(), UserData.GetAppIp(), metadataSnapshot);

                // 3. Fire the event to tell WinForms the dynamic state just changed
                StateUpdated?.Invoke(name, newValue);
            }
        }
    }
}