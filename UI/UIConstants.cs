

namespace WinForms_GUI_App.UI
{
    internal static class UIConstants
    {
        // Colors must be static readonly because Color is a struct, not a compile-time constant
        public static readonly Color DefaultHeaderColor = Color.White;

        // Strings and primitives can be const
        public const string DefaultFontFamily = "Segoe UI";

        // Standardized font sizes
        public const float Header1Size = 30f;
        public const float Header2Size = 24f;
        public const float Header3Size = 18f;
        public const float Header4Size = 14f;
        public const float Header5Size = 12f;
    }
}