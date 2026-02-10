using SongbookManagerMaui.Models;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class PreviewRepertoirePage : ContentPage
{
	public PreviewRepertoirePage(PreviewRepertoirePageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (PreviewRepertoirePageViewModel)BindingContext;
        await viewModel.LoadPageAsync();
    }
}