using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinForms_GUI_App.Models;
using WinForms_GUI_App.Utils;

namespace WinForms_GUI_App
{
    internal class AppState
    {
        public class Watcher : DynamicObject
        {
            // Equivalent to Python's self.__dict__
            private readonly Dictionary<string, object> state = new Dictionary<string, object>();

            // Equivalent to global app_metadata
            private static readonly Dictionary<string, object> appMetadata = new Dictionary<string, object>();

            // Equivalent to global app
            private static App app;

            // System.Text.Json natively handles Guids (UUIDs) and basic objects.
            // C# does not require a custom JSON Encoder for this specific payload.
            private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            public Watcher()
            {
                state["current_page"] = "Dashboard";
            }

            // Equivalent to Python's __setattr__
            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                string name = binder.Name;
                state.TryGetValue(name, out object currentValue);

                if (!Equals(currentValue, value))
                {
                    state[name] = value;
                    OnChange(name, value);
                }

                return true; // Indicates the member was successfully set
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return state.TryGetValue(binder.Name, out result);
            }

            private void OnChange(string name, object newValue)
            {
                appMetadata[name] = newValue;

                // Generate JSON for the specific change delta
                var changeDict = new Dictionary<string, object> { { name, newValue } };
                string changeJson = JsonSerializer.Serialize(changeDict, jsonOptions);

                Debug.WriteLine($"Triggered Update: {changeJson}");

                // Create a snapshot (shallow copy) of the current state to pass into the App model.
                // This prevents future state changes from mutating historical App instances.
                var metadataSnapshot = new Dictionary<string, object>(appMetadata);

                // Change to fetch values from generated file instead of hardcoding
                app = new App(UserData.GetAppId(), UserData.GetAppIp(), metadataSnapshot);

                // Serialize the dictionary solely for the console output verification
                string consoleOutputJson = JsonSerializer.Serialize(app.AppMetadata, jsonOptions);
                Debug.WriteLine($"app_id: {app.AppId},\napp_ip: {app.AppIp},\napp_metadata: {consoleOutputJson}\n");
            }
        }
    }
}
