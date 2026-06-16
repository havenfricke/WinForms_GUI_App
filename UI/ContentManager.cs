using System.Drawing;
using System.Windows.Forms;

namespace WinForms_GUI_App.UI
{
    internal class ContentManager
    {
        private Panel containerPanel;

        public Panel InitializeContentPanel()
        {
            containerPanel = new Panel();
            containerPanel.Dock = DockStyle.Fill;
            containerPanel.BackColor = SystemColors.Desktop;
            return containerPanel;
        }

        // Method to dynamically swap out the active page
        public void LoadView(UserControl view)
        {
            if (containerPanel == null) return;

            // Remove and dispose of the previous view to prevent memory leaks
            if (containerPanel.Controls.Count > 0)
            {
                containerPanel.Controls[0].Dispose();
                containerPanel.Controls.Clear();
            }

            // Configure and inject the new view
            view.Dock = DockStyle.Fill;
            containerPanel.Controls.Add(view);
        }
    }
}