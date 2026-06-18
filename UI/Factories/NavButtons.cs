internal class NavButtons
{
    public Button CreateNavButton(string text, EventHandler onClick)
    {
        Button navBtn = new Button();
        navBtn.Text = text;
        navBtn.Size = new Size(180, 40);
        navBtn.FlatStyle = FlatStyle.Flat;
        navBtn.ForeColor = Color.White;

        if (onClick != null)
        {
            navBtn.Click += onClick;
        }

        return navBtn;
    }
}