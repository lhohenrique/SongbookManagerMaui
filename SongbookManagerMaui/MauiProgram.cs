using Microsoft.Extensions.Logging;
using SongbookManagerMaui.Services;
using SongbookManagerMaui.ViewModels;
using SongbookManagerMaui.Views;

namespace SongbookManagerMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // ViewModels
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<ForgotPasswordPageViewModel>();
            builder.Services.AddSingleton<MusicPageViewModel>();
            builder.Services.AddTransient<PreviewMusicPageViewModel>();
            builder.Services.AddTransient<AddEditMusicPageViewModel>();
            builder.Services.AddSingleton<RepertoirePageViewModel>();
            builder.Services.AddSingleton<ProfilePageViewModel>();
            builder.Services.AddTransient<PreviewRepertoirePageViewModel>();
            builder.Services.AddTransient<AddEditRepertoirePageViewModel>();
            builder.Services.AddTransient<PlayRepertoirePageViewModel>();
            builder.Services.AddTransient<SharePageViewModel>();
            builder.Services.AddTransient<AdminPageViewModel>();
            builder.Services.AddTransient<ChangePasswordPageViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddSingleton<MusicPage>();
            builder.Services.AddTransient<SharePage>();
            builder.Services.AddTransient<AddEditMusicPage>();
            builder.Services.AddTransient<PreviewMusicPage>();
            builder.Services.AddSingleton<RepertoirePage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddTransient<PreviewRepertoirePage>();
            builder.Services.AddTransient<AddEditRepertoirePage>();
            builder.Services.AddTransient<PlayRepertoirePage>();
            builder.Services.AddTransient<SharePage>();
            builder.Services.AddTransient<PrivacyPolicyPage>();
            builder.Services.AddTransient<AdminPage>();
            builder.Services.AddTransient<ChangePasswordPage>();

            // Services
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IMusicService, MusicService>();
            builder.Services.AddSingleton<IKeyService, KeyService>();
            builder.Services.AddSingleton<IRepertoireService, RepertoireService>();

            return builder.Build();
        }
    }
}
