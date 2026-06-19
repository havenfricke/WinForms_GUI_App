

namespace WinForms_GUI_App.Models
{
    internal class AppMetadata
    {
        public string current_page { get; set; }

        public string arduino_data { get; set; }

        public string selected_port { get; set; }

        public AppMetadata(string currentPage, string arduinoData, string selectedPort) 
        { 
            current_page = currentPage;
            arduino_data = arduinoData;
            selected_port = selectedPort;
        }
    }
}
