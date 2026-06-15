using WinForms_GUI_App.UI;

namespace WinForms_GUI_App
{
    public partial class Main : Form
    {
        private void InitializeCustomComponents()
        {
            // 1. Initialize the Grid System
            TableLayoutPanel gridLayout = new TableLayoutPanel();
            gridLayout.Dock = DockStyle.Fill; // Makes the grid span the entire Form
            gridLayout.ColumnCount = 2;       // Define 2 columns
            gridLayout.RowCount = 2;          // Define 2 rows

            // Configure column and row sizing
            gridLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Column 0: 50% width
            gridLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Column 1: 50% width
            gridLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));           // Row 0: Heights map to content
            gridLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));      // Row 1: Exactly 50 pixels tall

            // 2. Instantiate UI Elements using the new Header class
            Header headerFactory = new Header();
            Label h1Label = headerFactory.One("Hello, WinForms!");
            Label h2Label = headerFactory.Two("This is Header Two", Color.LightBlue);

            // 3. Create the Button
            Button myActionBtn = new Button();
            myActionBtn.Text = "Click Me";
            myActionBtn.Size = new Size(100, 30);
            myActionBtn.BackColor = Color.LightGray;
            myActionBtn.Click += MyActionBtn_Click;

            // 4. Assign Controls to specific Grid Cells (Control, Column Index, Row Index)
            gridLayout.Controls.Add(h1Label, 0, 0);     // Top-Left cell
            gridLayout.Controls.Add(h2Label, 1, 0);     // Top-Right cell
            gridLayout.Controls.Add(myActionBtn, 0, 1); // Bottom-Left cell

            // 5. CRITICAL: Add the grid to the Form's Controls collection
            this.Controls.Add(gridLayout);
        }

        // 5. Define the Event Handler logic
        private void MyActionBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The custom button was clicked!");
        }
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(804, 450);
            Name = "Main";
            Text = "Main";
            ResumeLayout(false);
        }

        #endregion
    }
}
