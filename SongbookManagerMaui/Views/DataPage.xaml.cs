using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class DataPage : ContentPage
{
	public DataPage(DataPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (DataPageViewModel)BindingContext;
        await viewModel.LoadPageAsync();
    }
}