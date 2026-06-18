using WinForms_GUI_App.UI.Factories;

namespace WinForms_GUI_App.UI.Components
{
    internal class Navigation
    {
        // Only one nav system should exist, so we can use a singleton pattern here
        public static Navigation nav { get; } = new Navigation();

        // Define a public event that passes a string (the target page name)
        public event EventHandler<string> OnNavigate;

        public FlowLayoutPanel InitializeNavbar()
        {
            FlowLayoutPanel navPanel = new FlowLayoutPanel();
            navPanel.Dock = DockStyle.Fill;
            navPanel.BackColor = Color.FromArgb(45, 45, 48);
            navPanel.FlowDirection = FlowDirection.TopDown;
            navPanel.Padding = new Padding(0);

            Label navHeader = new Header().Two("Menu");

            NavButtons btnFactory = new NavButtons();

            // The component wires its own internal logic to the buttons
            Button homeBtn = btnFactory.CreateNavButton("Home", (sender, e) => RequestNavigation("Home"));
            Button settingsBtn = btnFactory.CreateNavButton("Settings", (sender, e) => RequestNavigation("Settings"));

            navPanel.Controls.Add(navHeader);
            navPanel.Controls.Add(homeBtn);
            navPanel.Controls.Add(settingsBtn);

            return navPanel;
        }

        // Internal logic that triggers the public event
        private void RequestNavigation(string targetPage)
        {
            // The null conditional operator (?.) ensures we only invoke if there are subscribers
            OnNavigate.Invoke(this, targetPage);
        }
    }
}