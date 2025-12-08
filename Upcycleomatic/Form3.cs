using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Upcycleomatic
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.Load += Form3_Load;
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            await webViewHome.EnsureCoreWebView2Async();

            string rootPath = Path.Combine(Application.StartupPath, "wwwroot");

            webViewHome.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "local",
                rootPath,
                CoreWebView2HostResourceAccessKind.Allow
            );

            webViewHome.CoreWebView2.WebMessageReceived += WebMessageReceived;

            // Load the dashboard HTML
            webViewHome.Source = new Uri("http://local/home.html");
        }

        private void WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string raw = e.TryGetWebMessageAsString();
            if (string.IsNullOrWhiteSpace(raw)) return;

            if (raw.Contains("\"action\":\"logout\""))
            {
                // Close dashboard → return to Form1
                this.Close();
                Owner?.Show();
            }
        }
    }
}
