using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class RepertoirePage : ContentPage
{
	public RepertoirePage(RepertoirePageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (RepertoirePageViewModel)BindingContext;
        await viewModel.LoadingPage();
    }

    private void RepertoireSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (RepertoirePageViewModel)BindingContext;
        viewModel.SearchCommand.Execute(null);
    }

}