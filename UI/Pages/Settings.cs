using System.Drawing;
using System.Windows.Forms;
using WinForms_GUI_App.UI;

namespace WinForms_GUI_App.UI.Pages
{
    internal class Settings : UserControl
    {
        public Settings()
        {
            BackColor = Color.Transparent; // Inherits background from the ContentPanel

            // Utilize your existing Header factory
            Header headerFactory = new Header();
            Label title = headerFactory.One("Settings");
            title.Location = new Point(20, 20);

            Controls.Add(title);
        }
    }
}