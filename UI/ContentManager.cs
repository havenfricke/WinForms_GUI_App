using WinForms_GUI_App.UI.Components;

namespace WinForms_GUI_App.UI
{
    internal class ContentManager
    {
        public static ContentManager manager { get; } = new ContentManager();

        // Reference the singleton's panel instance directly
        private Panel containerPanel = ContentPanel.panel.MainPanel;

        public void LoadView(UserControl view)
        {
            if (containerPanel == null) return;

            if (containerPanel.Controls.Count > 0)
            {
                containerPanel.Controls[0].Dispose();
                containerPanel.Controls.Clear();
            }

            view.Dock = DockStyle.Fill;
            containerPanel.Controls.Add(view);
        }
    }
}