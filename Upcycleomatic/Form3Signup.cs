using System;
using System.Drawing;
using System.Windows.Forms;

namespace Upcycleomatic
{
    public class Form3Signup : Form
    {
        public string UserName { get; private set; }

        TextBox txtUser;
        TextBox txtPass;
        TextBox txtConfirm;
        Button btnCreate;
        LinkLabel lnkBack;

        public Form3Signup()
        {
            Text = "Sign up";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 240);
            MaximizeBox = false;

            var lblUser = new Label() { Text = "Choose username", Left = 18, Top = 18, AutoSize = true };
            txtUser = new TextBox() { Left = 18, Top = 40, Width = 380 };

            var lblPass = new Label() { Text = "Password", Left = 18, Top = 74, AutoSize = true };
            txtPass = new TextBox() { Left = 18, Top = 96, Width = 380, UseSystemPasswordChar = true };

            var lblConfirm = new Label() { Text = "Confirm password", Left = 18, Top = 130, AutoSize = true };
            txtConfirm = new TextBox() { Left = 18, Top = 152, Width = 380, UseSystemPasswordChar = true };

            btnCreate = new Button() { Text = "Create account", Left = 300, Top = 188, Width = 98 };
            btnCreate.Click += (s, e) => CreateAccount();

            lnkBack = new LinkLabel() { Text = "Back to Login", Left = 18, Top = 188, AutoSize = true };
            lnkBack.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, lblConfirm, txtConfirm, btnCreate, lnkBack });
        }

        private void CreateAccount()
        {
            var u = txtUser.Text?.Trim() ?? "";
            var p = txtPass.Text ?? "";
            var c = txtConfirm.Text ?? "";

            if (u.Length < 3) { MessageBox.Show("Username must be at least 3 characters", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (p.Length < 4) { MessageBox.Show("Password too short", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (p != c) { MessageBox.Show("Passwords do not match", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            // Demo: no persistence; in real app save hashed creds
            UserName = u;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
