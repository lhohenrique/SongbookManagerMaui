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
            Routing.RegisterRoute(nameof(PreviewRepertoirePage),
                typeof(PreviewRepertoirePage));
            Routing.RegisterRoute(nameof(AddEditRepertoirePage),
                typeof(AddEditRepertoirePage));
            Routing.RegisterRoute(nameof(PlayRepertoirePage),
                typeof(PlayRepertoirePage));
            Routing.RegisterRoute(nameof(SharePage),
                typeof(SharePage));
            Routing.RegisterRoute(nameof(PrivacyPolicyPage),
                typeof(PrivacyPolicyPage));
            Routing.RegisterRoute(nameof(AdminPage),
                typeof(AdminPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage),
                typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(DataPage),
                typeof(DataPage));
        }
    }
}
