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

            // Views
            builder.Services.AddTransient<LoginPage>();

            // Services
            builder.Services.AddSingleton<IUserService, UserService>();

            return builder.Build();
        }
    }
}
