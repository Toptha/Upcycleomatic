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
            // Ensure WebView2 is ready
            await webViewHome.EnsureCoreWebView2Async();

            // Apply WinForms theme from Singleton
            SessionManager.Instance.ApplyTheme(this);

            // Map wwwroot to a virtual host
            string rootPath = Path.Combine(Application.StartupPath, "wwwroot");

            webViewHome.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "local",
                rootPath,
                CoreWebView2HostResourceAccessKind.Allow
            );

            // Listen for JS messages
            webViewHome.CoreWebView2.WebMessageReceived += WebMessageReceived;

            // Load dashboard
            webViewHome.Source = new Uri("http://local/home.html");
        }

        private void WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string raw = e.TryGetWebMessageAsString();
            if (string.IsNullOrWhiteSpace(raw)) return;

            /* ---------------- LOGOUT ---------------- */
            if (raw.Contains("\"action\":\"logout\""))
            {
                SessionManager.Instance.Logout();
                this.Close();
                Owner?.Show();
                return;
            }

            /* ------------- NAVIGATION --------------- */
            if (raw.Contains("\"action\":\"navigate\""))
            {
                if (raw.Contains("\"settings\""))
                {
                    webViewHome.CoreWebView2.Navigate("http://local/settings.html");
                    return;
                }

                if (raw.Contains("\"home\""))
                {
                    webViewHome.CoreWebView2.Navigate("http://local/home.html");
                    return;
                }
            }

            /* -------------- THEME ------------------- */
            if (raw.Contains("\"type\":\"theme\""))
            {
                if (raw.Contains("\"dark\""))
                    SessionManager.Instance.SetTheme(AppTheme.Dark);
                else
                    SessionManager.Instance.SetTheme(AppTheme.Light);

                // Apply to WinForms shell
                SessionManager.Instance.ApplyTheme(this);

                // Notify current web page
                webViewHome.CoreWebView2.PostWebMessageAsString(
                    SessionManager.Instance.CurrentTheme == AppTheme.Dark
                        ? "dark"
                        : "light"
                );
            }
        }
    }
}
