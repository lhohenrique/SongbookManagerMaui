using SongbookManagerMaui.Services;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class MusicPage : ContentPage
{
	public MusicPage(MusicPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }

    protected async override void OnAppearing()
    {
        var viewModel = (MusicPageViewModel)BindingContext;
        await viewModel.LoadingPage();
    }

    private void MusicSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (MusicPageViewModel)BindingContext;
        viewModel.SearchCommand.Execute(null);
    }
}