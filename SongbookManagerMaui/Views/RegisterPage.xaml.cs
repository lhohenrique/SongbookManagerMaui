using SongbookManagerMaui.Services;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class RegisterPage : ContentPage
{
    
    public RegisterPage(IUserService userService)
	{
		InitializeComponent();

        BindingContext = new RegisterPageViewModel(userService);
    }
}