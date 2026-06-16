namespace WinForms_GUI_App.UI.Pages
{
    internal class Home : UserControl
    {
        public Home()
        {
            BackColor = Color.Transparent; // Inherits background from the ContentPanel

            // Utilize your existing Header factory
            Header headerFactory = new Header();
            Label title = headerFactory.One("Home Dashboard");
            title.Location = new Point(20, 20);

            Controls.Add(title);
        }
    }
}