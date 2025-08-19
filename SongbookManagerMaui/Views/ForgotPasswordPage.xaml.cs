using SongbookManagerMaui.Services;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage(IUserService userService)
	{
		InitializeComponent();

        BindingContext = new ForgotPasswordPageViewModel(userService);
    }
}