using System;
using System.Drawing;
using System.Windows.Forms;
using Upcycleomatic;

namespace Upcycleomatic
{
    public class Form2Login : Form
    {
        public string UserName { get; private set; }

        TextBox txtUser;
        TextBox txtPass;
        Button btnLogin;
        LinkLabel lnkSignup;
        LinkLabel lnkHome;

        public Form2Login()
        {
            Text = "Log in";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 200);
            MaximizeBox = false;

            var lblUser = new Label() { Text = "Username", Left = 18, Top = 18, AutoSize = true };
            txtUser = new TextBox() { Left = 18, Top = 40, Width = 380 };

            var lblPass = new Label() { Text = "Password", Left = 18, Top = 74, AutoSize = true };
            txtPass = new TextBox() { Left = 18, Top = 96, Width = 380, UseSystemPasswordChar = true };

            btnLogin = new Button() { Text = "Log in", Left = 300, Top = 136, Width = 98 };
            btnLogin.Click += (s, e) => AttemptLogin();

            lnkSignup = new LinkLabel() { Text = "Don't have an account? Sign up", Left = 18, Top = 136, AutoSize = true };
            lnkSignup.Click += (s, e) => OpenSignup();

            lnkHome = new LinkLabel() { Text = "Back to Home", Left = 18, Top = 162, AutoSize = true };
            lnkHome.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, btnLogin, lnkSignup, lnkHome });
        }

        private void AttemptLogin()
        {
            var u = txtUser.Text?.Trim() ?? "";
            var p = txtPass.Text ?? "";
            if (u.Length == 0 || p.Length == 0)
            {
                MessageBox.Show("Enter both username and password", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Demo: accept any credentials length >= 3
            if (p.Length < 3)
            {
                MessageBox.Show("Password too short", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UserName = u;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OpenSignup()
        {
            using var signup = new Form3Signup();
            var dr = signup.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                // if user just created account, auto-fill username and close as success
                txtUser.Text = signup.UserName;
                MessageBox.Show("Account created. Please log in (or click Log in)", "Signed up", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
