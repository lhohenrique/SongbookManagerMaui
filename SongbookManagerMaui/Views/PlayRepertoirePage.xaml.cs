using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class PlayRepertoirePage : ContentPage
{
	public PlayRepertoirePage(PlayRepertoirePageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (PlayRepertoirePageViewModel)BindingContext;
        await viewModel.LoadPageAsync();
    }
}