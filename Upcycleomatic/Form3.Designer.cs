namespace Upcycleomatic
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewHome;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null) components.Dispose();
                if (webViewHome != null) webViewHome.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            webViewHome = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webViewHome).BeginInit();
            SuspendLayout();
            // 
            // webViewHome
            // 
            webViewHome.AllowExternalDrop = true;
            webViewHome.BackColor = Color.Wheat;
            webViewHome.CreationProperties = null;
            webViewHome.DefaultBackgroundColor = Color.FromArgb(11, 20, 55);
            webViewHome.Dock = DockStyle.Fill;
            webViewHome.Location = new Point(0, 0);
            webViewHome.Name = "webViewHome";
            webViewHome.Size = new Size(1000, 660);
            webViewHome.TabIndex = 1;
            webViewHome.ZoomFactor = 1D;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(11, 20, 55);
            ClientSize = new Size(1000, 660);
            Controls.Add(webViewHome);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimumSize = new Size(780, 480);
            Name = "Form3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Home — Upcycleomatic";
            ((System.ComponentModel.ISupportInitialize)webViewHome).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
