using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        var viewModel = (ProfilePageViewModel)BindingContext;
        viewModel.OnAppearing();
    }
}