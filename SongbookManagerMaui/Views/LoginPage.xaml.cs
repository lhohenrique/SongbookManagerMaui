using SongbookManagerMaui.Services;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(IUserService userService)
	{
		InitializeComponent();

        BindingContext = new LoginPageViewModel(userService);
    }

    private void LoginPageAppearing(object sender, EventArgs e)
    {
        var viewModel = (LoginPageViewModel)BindingContext;
        viewModel.OnAppearingAsync();
    }
}