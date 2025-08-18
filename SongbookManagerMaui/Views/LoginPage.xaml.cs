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

    protected override void OnAppearing()
    {
        var viewModel = (LoginPageViewModel)BindingContext;
        //viewModel.OnAppearingAsync();
    }
}