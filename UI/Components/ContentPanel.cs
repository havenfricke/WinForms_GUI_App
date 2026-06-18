namespace WinForms_GUI_App.UI.Components
{
    internal class ContentPanel
    {
        public static ContentPanel panel { get; } = new ContentPanel();

        // Expose the single UI element
        public Panel MainPanel { get; private set; }

        // Initialize it once in the constructor
        private ContentPanel()
        {
            MainPanel = new Panel();
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.BackColor = SystemColors.Desktop;
        }
    }
}