

namespace WinForms_GUI_App.Models
{
    internal class App
    {
        public string AppId { get; set; }
        public string AppIp { get; set; }
        public AppMetadata AppMetadata { get; set; }

        public App(string appId, string appIp, AppMetadata appMetadata)
        {
            AppId = appId;
            AppIp = appIp;
            AppMetadata = appMetadata;
        }
    }
}
