using SongbookManagerMaui.Models;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class AdminPage : ContentPage
{
	public AdminPage(AdminPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (AdminPageViewModel)BindingContext;
        await viewModel.LoadingPageAsync();
    }

    private async void UserMenuItem_Clicked(object sender, EventArgs e)
    {
        var user = ((MenuItem)sender).BindingContext as User;
        if (user == null)
        {
            return;
        }

        var viewModel = (AdminPageViewModel)BindingContext;
        await viewModel.RemoveUserAction(user);
    }

    private async void MusicMenuItem_Clicked(object sender, EventArgs e)
    {
        var music = ((MenuItem)sender).BindingContext as Music;
        if (music == null)
        {
            return;
        }

        var viewModel = (AdminPageViewModel)BindingContext;
        await viewModel.RemoveMusicAction(music);
    }
}