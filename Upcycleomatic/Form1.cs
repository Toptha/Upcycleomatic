using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Upcycleomatic;

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
            // ensure wwwroot/data exists
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "wwwroot", "data"));

            // initialize WebView2
            try
            {
                await webView21.EnsureCoreWebView2Async();
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 init failed: " + ex.Message);
                return;
            }

            webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            // open landing page
            var indexPath = Path.Combine(Application.StartupPath, "wwwroot", "index.html");
            webView21.CoreWebView2.Navigate(new Uri(indexPath).AbsoluteUri);
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
                        // navigate back to landing
                        webView21.CoreWebView2.Navigate(new Uri(Path.Combine(Application.StartupPath, "wwwroot", "index.html")).AbsoluteUri);
                        break;
                }
            }
            catch { /* ignore bad messages */ }
        }

        private void OpenLogin()
        {
            using var login = new Form2Login();
            var dr = login.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                // send auth ok and username to web page, then navigate to app.html
                var payload = JsonSerializer.Serialize(new { type = "auth", status = "ok", user = login.UserName }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                webView21.CoreWebView2.PostWebMessageAsString(payload);

                // navigate
                var appPath = Path.Combine(Application.StartupPath, "wwwroot", "app.html");
                webView21.CoreWebView2.Navigate(new Uri(appPath).AbsoluteUri);
            }
        }

        private void OpenSignup()
        {
            using var signup = new Form3Signup();
            var dr = signup.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                // simulate automatic login after signup
                var payload = JsonSerializer.Serialize(new { type = "auth", status = "ok", user = signup.UserName }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                webView21.CoreWebView2.PostWebMessageAsString(payload);

                var appPath = Path.Combine(Application.StartupPath, "wwwroot", "app.html");
                webView21.CoreWebView2.Navigate(new Uri(appPath).AbsoluteUri);
            }
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void webView21_Click_1(object sender, EventArgs e)
        {

        }
    }
}
