namespace WinForms_GUI_App.UI
{
    internal class ContentPanel
    {
        public Panel InitializeContentPanel()
        {
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = SystemColors.Desktop;
            return contentPanel;
        }
    }
}
