using System.Drawing;
using System.Windows.Forms;
using WinForms_GUI_App.UI;
using WinForms_GUI_App.UI.Pages;

namespace WinForms_GUI_App
{
    partial class Main
    {
        private ContentManager contentManager;
        private void InitializeCustomComponents()
        {
            TableLayoutPanel mainGrid = new TableLayoutPanel();
            mainGrid.Dock = DockStyle.Fill;
            mainGrid.ColumnCount = 2;
            mainGrid.RowCount = 1;
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Navigation navComponent = new Navigation();
            navComponent.OnNavigate += NavigationComponent_OnNavigate;

            // 1. Initialize the ContentManager
            contentManager = new ContentManager();

            FlowLayoutPanel navPanel = navComponent.InitializeNavbar();
            Panel contentPanel = contentManager.InitializeContentPanel();

            mainGrid.Controls.Add(navPanel, 0, 0);
            mainGrid.Controls.Add(contentPanel, 1, 0);
            this.Controls.Add(mainGrid);

            // 2. Load the default startup view
            contentManager.LoadView(new Home());
        }

        // 3. Handle the routing request
        private void NavigationComponent_OnNavigate(object sender, string targetPage)
        {
            System.Diagnostics.Debug.WriteLine($"Loading view: {targetPage}");

            switch (targetPage)
            {
                case "Home":
                    contentManager.LoadView(new Home());
                    break;
                case "Settings":
                    contentManager.LoadView(new Settings());
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"View '{targetPage}' not found.");
                    break;
            }
        }

        #region Windows Form Designer generated code
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(804, 450);
            Name = "App Name";
            Text = "App Name";
            ResumeLayout(false);
        }
        #endregion
    }
}