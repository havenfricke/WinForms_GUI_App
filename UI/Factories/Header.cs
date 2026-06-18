

namespace WinForms_GUI_App.UI.Factories
{
    internal class Header
    {
        // Factory method to handle the repetitive label creation logic
        private Label CreateHeaderLabel(string text, float fontSize, Color foreColor)
        {
            Label label = new Label();
            label.Text = text;
            label.ForeColor = foreColor;
            label.AutoSize = true;
            label.Font = new Font(UIConstants.DefaultFontFamily, fontSize, FontStyle.Bold);

            // Note: Location is deliberately omitted. The grid system will manage positioning.
            return label;
        }

        // Overloaded methods utilize UIConstants for sizing and allow optional color overrides
        public Label One(string text, Color? foreColor = null)
        {
            return CreateHeaderLabel(text, UIConstants.Header1Size, foreColor ?? UIConstants.DefaultHeaderColor);
        }

        public Label Two(string text, Color? foreColor = null)
        {
            return CreateHeaderLabel(text, UIConstants.Header2Size, foreColor ?? UIConstants.DefaultHeaderColor);
        }

        public Label Three(string text, Color? foreColor = null)
        {
            return CreateHeaderLabel(text, UIConstants.Header3Size, foreColor ?? UIConstants.DefaultHeaderColor);
        }

        public Label Four(string text, Color? foreColor = null)
        {
            return CreateHeaderLabel(text, UIConstants.Header4Size, foreColor ?? UIConstants.DefaultHeaderColor);
        }

        public Label Five(string text, Color? foreColor = null)
        {
            return CreateHeaderLabel(text, UIConstants.Header5Size, foreColor ?? UIConstants.DefaultHeaderColor);
        }
    }
}