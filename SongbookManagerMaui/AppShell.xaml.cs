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
            Routing.RegisterRoute(nameof(SharePage),
                typeof(SharePage));
            Routing.RegisterRoute(nameof(AddEditMusicPage),
                typeof(AddEditMusicPage));
            Routing.RegisterRoute(nameof(PreviewMusicPage),
                typeof(PreviewMusicPage));
        }
    }
}
