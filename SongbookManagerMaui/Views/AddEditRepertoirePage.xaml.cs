using SongbookManagerMaui.Models;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class AddEditRepertoirePage : ContentPage
{
	public AddEditRepertoirePage(AddEditRepertoirePageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (AddEditRepertoirePageViewModel)BindingContext;
        await viewModel.PopulateRepertoireFieldsAsync();
    }
}