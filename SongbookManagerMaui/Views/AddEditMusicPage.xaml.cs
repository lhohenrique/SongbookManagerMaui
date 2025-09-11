using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class AddEditMusicPage : ContentPage
{
	public AddEditMusicPage(AddEditMusicPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (AddEditMusicPageViewModel)BindingContext;
        await viewModel.PopulateMusicFieldsAsync();
    }
}