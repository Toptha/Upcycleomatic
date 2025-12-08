using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Upcycleomatic
{
    public partial class Form2 : Form
    {
        private const string VALID_EMAIL = "preetham@gmail.com";
        private const string VALID_PASSWORD = "1234567";

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {

            await webView21.EnsureCoreWebView2Async();

            string rootPath = Path.Combine(Application.StartupPath, "wwwroot");
            if (!Directory.Exists(rootPath))
            {
                MessageBox.Show($"wwwroot folder not found at:\n{rootPath}\n\nMake sure your HTML/CSS are set to 'Copy to Output Directory: Copy always'.",
                                "Missing wwwroot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "local",
                rootPath,
                CoreWebView2HostResourceAccessKind.Allow
            );

            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView21.Source = new Uri("http://local/login.html");
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.TryGetWebMessageAsString();

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("type", out var t) && t.GetString() == "login")
                {
                    string email = root.GetProperty("email").GetString() ?? "";
                    string password = root.GetProperty("password").GetString() ?? "";

                    if (IsValidCredentials(email, password))
                    {
                        this.Invoke((Action)(() =>
                        {
                            var home = new Form3();
                            home.FormClosed += (s, args) =>
                            {
                                this.Show();
                                webView21.Reload();
                            };

                            home.Show();
                            this.Hide();
                        }));
                    }
                    else
                    {
                        webView21.CoreWebView2.PostWebMessageAsString("{\"type\":\"login_result\",\"ok\":false}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("WebMessage parsing failed: " + ex.Message);
            }
        }

        private bool IsValidCredentials(string email, string password)
        {

            return string.Equals(email?.Trim(), VALID_EMAIL, StringComparison.OrdinalIgnoreCase)
                && password == VALID_PASSWORD;
        }
    }
}
