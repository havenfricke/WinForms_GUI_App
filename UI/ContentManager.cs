using System.Drawing;
using System.Windows.Forms;

namespace WinForms_GUI_App.UI
{
    internal class ContentManager
    {
        private Panel _containerPanel;

        public Panel InitializeContentPanel()
        {
            _containerPanel = new Panel();
            _containerPanel.Dock = DockStyle.Fill;
            _containerPanel.BackColor = SystemColors.Desktop;
            return _containerPanel;
        }

        // Method to dynamically swap out the active page
        public void LoadView(UserControl view)
        {
            if (_containerPanel == null) return;

            // Remove and dispose of the previous view to prevent memory leaks
            if (_containerPanel.Controls.Count > 0)
            {
                _containerPanel.Controls[0].Dispose();
                _containerPanel.Controls.Clear();
            }

            // Configure and inject the new view
            view.Dock = DockStyle.Fill;
            _containerPanel.Controls.Add(view);
        }
    }
}