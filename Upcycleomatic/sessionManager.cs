using System.Drawing;
using System.Windows.Forms;

namespace Upcycleomatic
{
    public sealed class SessionManager
    {
        // SINGLE instance
        private static SessionManager _instance;

        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SessionManager();
                return _instance;
            }
        }

        // Private constructor = no one can create new objects
        private SessionManager()
        {
            // default values
            IsLoggedIn = false;
            Username = "";
            CurrentTheme = AppTheme.Light;
        }

        // SESSION DATA
        public bool IsLoggedIn { get; private set; }
        public string Username { get; private set; }
        public AppTheme CurrentTheme { get; private set; }

        // LOGIN
        public void Login(string username)
        {
            Username = username;
            IsLoggedIn = true;
        }

        // LOGOUT
        public void Logout()
        {
            Username = "";
            IsLoggedIn = false;
        }

        // THEME CHANGE
        public void SetTheme(AppTheme theme)
        {
            CurrentTheme = theme;
        }

        // APPLY THEME TO ANY FORM
        public void ApplyTheme(Form form)
        {
            if (CurrentTheme == AppTheme.Dark)
            {
                form.BackColor = Color.FromArgb(30, 30, 30);
                form.ForeColor = Color.White;
            }
            else
            {
                form.BackColor = Color.White;
                form.ForeColor = Color.Black;
            }
        }
    }

    public enum AppTheme
    {
        Light,
        Dark
    }
}
