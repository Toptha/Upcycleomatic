using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Upcycleomatic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            SessionManager.Instance.ApplyTheme(this);
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "wwwroot", "data"));
            try
            {
                await webView21.EnsureCoreWebView2Async();
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 init failed: " + ex.Message);
                return;
            }

            string root = Path.Combine(Application.StartupPath, "wwwroot");
            if (!Directory.Exists(root))
            {
                MessageBox.Show($"wwwroot not found at:\n{root}\n\nMake sure your HTML/CSS files are set to 'Copy to Output Directory: Copy always'.",
                    "Missing wwwroot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "local",
                root,
                CoreWebView2HostResourceAccessKind.Allow
            );

            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView21.CoreWebView2.Navigate("http://local/index.html");
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var raw = e.TryGetWebMessageAsString();
            if (string.IsNullOrWhiteSpace(raw)) return;

            try
            {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;
                if (!root.TryGetProperty("action", out var act)) return;
                var action = act.GetString();

                switch (action)
                {
                    case "openLogin":
                        OpenLogin();
                        break;
                    case "openSignup":
                        OpenSignup();
                        break;
                    case "goHome":
                        webView21.CoreWebView2.Navigate("http://local/index.html");
                        break;
                }
            }
            catch
            {
                // ignore bad messages
            }
        }

        private void OpenLogin()
        {
           
            var login = new Form2(); 
            login.FormClosed += (s, e) =>
            {
                //refresh
            };
            login.Show(this);
        }

        private void OpenSignup()
        {
            var signup = new Form3Signup();
            var dr = signup.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                var payload = JsonSerializer.Serialize(new { type = "auth", status = "ok", user = signup.UserName });
                webView21.CoreWebView2.PostWebMessageAsString(payload);
                webView21.CoreWebView2.Navigate("http://local/app.html");
            }
        }

        private void webView21_Click(object sender, EventArgs e) { }
    }
}
