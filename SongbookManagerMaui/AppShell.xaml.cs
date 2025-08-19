using SongbookManagerMaui.Views;

namespace SongbookManagerMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage),
                typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage),
                typeof(ForgotPasswordPage));
        }
    }
}
